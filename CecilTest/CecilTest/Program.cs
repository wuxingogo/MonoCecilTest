using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  Inject my dll "WuxingogoEditor"
/// </summary>
namespace CecilTest
{
    class Program
    {
        static void Main( string[] args )
        {
            ModuleDefinition module = ModuleDefinition.ReadModule( "WuxingogoEditor.dll" );
            var assembiy = module.Assembly;
            foreach( var item in module.Types )
            {
                if( item.Name.Equals( "XQucickSetPrefs" ) )
                {
                    Console.WriteLine( "Seach XQuickSetPrefs" );
                    var definition = item;
                    foreach( var method in definition.Methods )
                    {
                        if( method.Name.Equals( "OnXGUI" ) )
                        {
                            foreach( Instruction inst in method.Body.Instructions )
                            {
                                if( inst.OpCode.Name == "ldstr" && ( ( string )inst.Operand ).Contains( "Value" )) // if find string contain"Value" ===>change to "Hello Value!";
                                {
                                    inst.Operand = "Hello Value";
                                }

                            }
                                Console.WriteLine( "Seach OnXGUI" );
                            var ins = method.Body.Instructions[0];
                            var worker = method.Body.GetILProcessor();
                            worker.InsertBefore( ins, worker.Create( OpCodes.Ldstr, "Method start…" ) );
                            worker.InsertBefore( ins, worker.Create( OpCodes.Call,
                                assembiy.MainModule.Import( typeof( Console ).GetMethod( "WriteLine", new Type[] { typeof( string ) } ) ) ) );
                            ins = method.Body.Instructions[method.Body.Instructions.Count - 1];

                            worker.InsertBefore( ins, worker.Create( OpCodes.Ldstr, "Method finish…" ) );
                            worker.InsertBefore( ins, worker.Create( OpCodes.Call,
                                assembiy.MainModule.Import( typeof( Console ).GetMethod( "WriteLine", new Type[] { typeof( string ) } ) ) ) );
                            // SaveAssembly is obsolete
                            module.Write( "IL.dll" );
                            break;
                        }
                    }
                    // SaveAssembly is obsolete
                    //AssemblyFactory.SaveAssembly( assembiy, "IL_" + args[0] );


                }

            }
           

            // print all type

            //foreach( TypeDefinition type in module.Types )
            //{
            //    if( !type.IsPublic )
            //        continue;

            //    Console.WriteLine( type.FullName );
            //}
            Console.Read();
        }
    }
}
