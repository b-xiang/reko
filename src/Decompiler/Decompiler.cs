#region License
/* Copyright (C) 1999-2015 John K�ll�n.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Reko.Core;
using Reko.Core.Lib;
using Reko.Core.Output;
using Reko.Core.Serialization;
using Reko.Core.Services;
using Reko.Core.Types;
using Reko.Scanning;
using Reko.Loading;
using Reko.Analysis;
using Reko.Structure;
using Reko.Typing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Reko.Core.Assemblers;
using System.Threading;

namespace Reko
{
    public interface IDecompiler
    {
        Project Project { get; }

        bool Load(string fileName);
        void LoadRawImage(string fileName, IProcessorArchitecture arch, Platform platform, Address addrBase);
        void ScanPrograms();
        ProcedureBase ScanProcedure(ProgramAddress paddr);
        void AnalyzeDataFlow();
        void ReconstructTypes();
        void StructureProgram();

        void Assemble(string file, Assembler asm);
    }

	/// <summary>
	/// The main driver class for decompilation of binaries. 
	/// </summary>
	/// <remarks>
	/// This class is named this way as the previous name 'Decompiler' causes C# to get confused
	/// between the namespace and the class name.
	/// </remarks>
	public class DecompilerDriver : IDecompiler
	{
		private DecompilerHost host;
		private readonly ILoader loader;
		private IScanner scanner;
        private DecompilerEventListener eventListener;
        private IServiceProvider services;

        public DecompilerDriver(ILoader ldr, DecompilerHost host, IServiceProvider services)
        {
            if (ldr == null)
                throw new ArgumentNullException("ldr");
            if (host == null)
                throw new ArgumentNullException("host");
            if (services == null)
                throw new ArgumentNullException("services");
            this.loader = ldr;
            this.host = host;
            this.services = services;
            this.eventListener = services.GetService<DecompilerEventListener>();
        }

        public Project Project { get { return project; } set { project = value; ProjectChanged.Fire(this); } }
        public event EventHandler ProjectChanged;
        private Project project;

        /// <summary>
        /// Main entry point of the decompiler. Loads, decompiles, and outputs the results.
        /// </summary>
        public void Decompile(string filename)
        {
            try
            {
                Load(filename);
                ScanPrograms();
                AnalyzeDataFlow();
                ReconstructTypes();
                StructureProgram();
                WriteDecompilerProducts();
            }
            catch (Exception ex)
            {
                eventListener.Error(
                    new NullCodeLocation(filename),
                    ex,
                    "An internal error occurred while decompiling.");
            }
            finally
            {
                eventListener.ShowStatus("Decompilation finished.");
            }
        }

        ///<summary>
        /// Determines the signature of the procedures,
		/// the locations and types of all the values in the program.
		///</summary>
        public virtual void AnalyzeDataFlow()
        {
            foreach (var program in project.Programs)
            {
                eventListener.ShowStatus("Performing interprocedural analysis.");
                var dfa = new DataFlowAnalysis(program, eventListener);
                dfa.UntangleProcedures();

                dfa.BuildExpressionTrees();
                host.WriteIntermediateCode(program, writer => { EmitProgram(program, dfa, writer); });
            }
            eventListener.ShowStatus("Interprocedural analysis complete.");
        }

        public void DumpAssembler(Program program, TextWriter wr)
        {
            if (wr == null || program.Architecture == null)
                return;
            Dumper dump = new Dumper(program.Architecture);
            dump.Dump(program, program.ImageMap, wr);
        }

        private void EmitProgram(Program program, DataFlowAnalysis dfa, TextWriter output)
        {
            if (output == null)
                return;
            foreach (Procedure proc in program.Procedures.Values)
            {
                if (dfa != null)
                {
                    ProcedureFlow flow = dfa.ProgramDataFlow[proc];
                    TextFormatter f = new TextFormatter(output);
                    if (flow.Signature != null)
                        flow.Signature.Emit(proc.Name, ProcedureSignature.EmitFlags.LowLevelInfo, f);
                    else if (proc.Signature != null)
                        proc.Signature.Emit(proc.Name, ProcedureSignature.EmitFlags.LowLevelInfo, f);
                    else
                        output.Write("Warning: no signature found for {0}", proc.Name);
                    output.WriteLine();
                    flow.Emit(program.Architecture, output);
                    foreach (Block block in new DfsIterator<Block>(proc.ControlGraph).PostOrder().Reverse())
                    {
                        if (block == null)
                            continue;
                        block.Write(output); output.Flush();
                        BlockFlow bf = dfa.ProgramDataFlow[block];
                        if (bf != null)
                        {
                            bf.Emit(program.Architecture, output);
                            output.WriteLine();
                        }
                    }
                }
                else
                {
                    proc.Write(false, output);
                }
                output.WriteLine();
                output.WriteLine();
            }
            output.Flush();
        }

        /// <summary>
        /// Loads (or assembles) the decompiler project. If a binary file is specified instead,
        /// we create a simple project for the file.
        /// </summary>
        /// <returns>True if what was loaded was an actual project</returns>
        /// <param name="program"></param>
        /// <param name="cfg"></param>
        public bool Load(string fileName)
        {
            eventListener.ShowStatus("Loading source program.");
            byte[] image = loader.LoadImageBytes(fileName, 0);
            var projectLoader = new ProjectLoader(this.services, loader);
            projectLoader.ProgramLoaded += (s, e) => { RunScriptOnProgramImage(e.Program, e.Program.OnLoadedScript); };
            Project = projectLoader.LoadProject(fileName, image);
            bool isProject;
            if (Project != null)
            {
                isProject = true;
            }
            else 
            {
                var program = loader.LoadExecutable(fileName, image, null);
                Project = CreateDefaultProject(fileName, program);
                isProject = false;
            }
            eventListener.ShowStatus("Source program loaded.");
            return isProject;
        }

        public void RunScriptOnProgramImage(Program program, Script_v2 script)
        {
            if (script == null || !script.Enabled)
                return;
            IScriptInterpreter interpreter;
            try
            {
                //$TODO: should be in the config file, yeah.
                var type = Type.GetType("Reko.ImageLoaders.OdbgScript.OllyLang,Reko.ImageLoaders.OdbgScript");
                interpreter = (IScriptInterpreter) Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                eventListener.Error(new NullCodeLocation(""), ex, string.Format("Unable to load script interpreter {0}."));
                return;
            }

            try
            {
                interpreter.LoadFromString(script.Script, null);
                interpreter.Run();
            } catch (Exception ex)
            {
                eventListener.Error(new NullCodeLocation(""), ex, string.Format("An error occurred while running the script."));
            }
        }

        public void Assemble(string fileName, Assembler asm)
        {
            eventListener.ShowStatus("Assembling program.");
            byte[] image = loader.LoadImageBytes(fileName, 0);
            var program = loader.AssembleExecutable(fileName, asm, null);
            Project = CreateDefaultProject(fileName, program);
            eventListener.ShowStatus("Assembled program.");
        }

        /// <summary>
        /// Loads a program into memory, but performs no relocations.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arch"></param>
        /// <param name="platform"></param>
        public void LoadRawImage(string fileName, IProcessorArchitecture arch, Platform platform, Address addrBase)
        {
            eventListener.ShowStatus("Loading raw bytes.");
            byte[] image = loader.LoadImageBytes(fileName, 0);
            var loadedImage = new LoadedImage(addrBase, image);
            var program = new Program(
                loadedImage,
                loadedImage.CreateImageMap(),
                arch,
                platform);
            program.Name = Path.GetFileName(fileName);
            Project = CreateDefaultProject(fileName, program);
            eventListener.ShowStatus("Raw bytes loaded.");
        }

        protected Project CreateDefaultProject(string fileName, Program program)
        {
            program.Filename = fileName;
            program.EnsureFilenames(fileName);

            var project = new Project
            {
                Programs = { program },
            };
            return project;
        }

		/// <summary>
		/// Extracts type information from the typeless rewritten programs.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="ivs"></param>
        public void ReconstructTypes()
        {
            foreach (var program in Project.Programs)
            {
                TypeAnalyzer analyzer = new TypeAnalyzer(eventListener);
                try
                {
                    try
                    {
                        analyzer.RewriteProgram(program);
                    }
                    catch (Exception ex)
                    {
                        eventListener.Error(new NullCodeLocation(""), ex, "Error when reconstructing types.");
                    }
                } 
                finally
                {
                    host.WriteTypes(program, analyzer.WriteTypes);
                }
            }
        }

        public void WriteDecompiledProcedures(Program program, TextWriter w)
        {
            WriteHeaderComment(Path.GetFileName(program.OutputFilename), program, w);
            w.WriteLine("#include \"{0}\"", Path.GetFileName(program.TypesFilename));
            w.WriteLine();
            var fmt = new AbsynCodeFormatter(new TextFormatter(w));
            foreach (Procedure proc in program.Procedures.Values)
            {
                try
                {
                    fmt.Write(proc);
                    w.WriteLine();
                }
                catch (Exception ex)
                {
                    w.WriteLine();
                    w.WriteLine("// Exception {0} when writing procedure.", ex.Message);
                }
            }
        }

        public void WriteGlobals(Program program, TextWriter w)
        {
            WriteHeaderComment(Path.GetFileName(program.OutputFilename), program, w);
            w.WriteLine("#include \"{0}\"", Path.GetFileName(program.TypesFilename));
            w.WriteLine();
            var gdw = new GlobalDataWriter(program, services);
            gdw.WriteGlobals(new TextFormatter(w));
            w.WriteLine();
        }
    
        public void WriteDecompiledTypes(Program program, TextWriter w)
        {
            WriteHeaderComment(Path.GetFileName(program.TypesFilename), program, w);
            w.WriteLine("/*"); program.TypeStore.Write(w); w.WriteLine("*/");
            var tf = new TextFormatter(w)
            {
                Indentation = 0,
            };
            TypeFormatter fmt = new TypeFormatter(tf, false);
            foreach (EquivalenceClass eq in program.TypeStore.UsedEquivalenceClasses)
            {
                if (eq.DataType != null)
                {
                    w.Write("typedef ");
                    fmt.Write(eq.DataType, eq.Name);
                    w.WriteLine(";");
                    w.WriteLine();
                }
            }
        }

        /// <summary>
        /// Starts a scan at address <paramref name="addr"/> on the user's request.
        /// </summary>
        /// <param name="addr"></param>
        /// <returns>a ProcedureBase, because the target procedure may have been a thunk or 
        /// an linked procedure the user has decreed not decompileable.</returns>
		public ProcedureBase ScanProcedure(ProgramAddress paddr)
		{
			if (scanner == null)        //$TODO: it's unfortunate that we depend on the scanner of the Decompiler class.
				scanner = CreateScanner(paddr.Program, eventListener);
			return scanner.ScanProcedure(paddr.Address, null, paddr.Program.Architecture.CreateProcessorState());
		}

		/// <summary>
		/// Generates the control flow graph and finds executable code in each program.
		/// </summary>
		/// <param name="program">the program whose flow graph we seek</param>
		/// <param name="cfg">configuration information</param>
		public void ScanPrograms()
		{
			if (Project.Programs.Count == 0)
				throw new InvalidOperationException("Programs must be loaded first.");

            foreach (Program program in Project.Programs)
            {
                ScanProgram(program);
            }
		}

        private void ScanProgram(Program program)
        {
            try
            {
                eventListener.ShowStatus("Rewriting reachable machine code.");
                scanner = CreateScanner(program, eventListener);
                foreach (EntryPoint ep in program.EntryPoints)
                {
                    scanner.EnqueueEntryPoint(ep);
                }
                foreach (Procedure_v1 up in program.UserProcedures.Values)
                {
                    scanner.EnqueueUserProcedure(up);
                }
                foreach (var addr in program.FunctionHints)
                {
                    scanner.EnqueueProcedure(addr);
                }
                //scanner.ScanImage();

                if (true || //$DEBUG
                    program.Options.HeuristicScanning)
                {
                    eventListener.ShowStatus("Finding machine code using heuristics.");
                    scanner.ScanImageHeuristically();
                }
                eventListener.ShowStatus("Finished rewriting reachable machine code.");
            }
            finally
            {
                host.WriteDisassembly(program, w => DumpAssembler(program, w));
                host.WriteIntermediateCode(program, w => EmitProgram(program, null, w));
            }
        }

        public IDictionary<Address, ProcedureSignature> LoadCallSignatures(
            Program program, 
            ICollection<SerializedCall_v1> serializedCalls)
        {
            return
                serializedCalls
                .Where(sc => sc != null && sc.Signature != null)
                .Select(sc =>
                {
                    var sser = program.Platform.CreateProcedureSerializer(
                        new TypeLibraryLoader(program.Platform, true), null);
                    Address addr;
                    if (program.Architecture.TryParseAddress(sc.InstructionAddress, out addr))
                    {
                        return new KeyValuePair<Address, ProcedureSignature>(
                            addr,
                            sser.Deserialize(sc.Signature, program.Architecture.CreateFrame()));
                    }
                    else
                        return new KeyValuePair<Address, ProcedureSignature>(null, null);
                })
                .ToDictionary(item => item.Key, item => item.Value);
        }

        private IScanner CreateScanner(Program program, DecompilerEventListener eventListener)
        {
            return new Scanner(
                program, 
                LoadCallSignatures(program, program.UserCalls.Values),
                new ImportResolver(project),
                eventListener);
        }

        /// <summary>
        /// Extracts structured program constructs out of snarled goto nests, if possible.
        /// Since procedures are now independent of each other, this analysis
        /// is done one procedure at a time.
        /// </summary>
        public void StructureProgram()
		{
            foreach (var program in project.Programs)
            {
                int i = 0;
                foreach (Procedure proc in program.Procedures.Values)
                {
                    try
                    {
                        eventListener.ShowProgress("Rewriting procedures to high-level language.", i, program.Procedures.Values.Count);
                        ++i;
                        IStructureAnalysis sa = new StructureAnalysis(eventListener, program, proc);
                        sa.Structure();
                    }
                    catch (Exception e)
                    {
                        eventListener.Error(
                            eventListener.CreateProcedureNavigator(program, proc),
                            e,
                            "An error occurred while rewriting procedure to high-level language.");
                    }
                }
                WriteDecompilerProducts();
            }
			eventListener.ShowStatus("Rewriting complete.");
		}

		public void WriteDecompilerProducts()
		{
            foreach (var program in Project.Programs)
            {
                host.WriteTypes(program, w => WriteDecompiledTypes(program, w));
                host.WriteDecompiledCode(program, w => WriteDecompiledProcedures(program, w));
                host.WriteGlobals(program, w => WriteGlobals(program, w));
            }
		}

		public void WriteHeaderComment(string filename, Program program, TextWriter w)
		{
			w.WriteLine("// {0}", filename);
			w.WriteLine("// Generated by decompiling {0}", program.Filename);
			w.WriteLine("// using Decompiler version {0}.", AssemblyMetadata.AssemblyFileVersion);
			w.WriteLine();
		}
	}
}
