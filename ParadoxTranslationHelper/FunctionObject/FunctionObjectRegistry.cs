using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.FunctionObject
{
    public sealed class FunctionObjectRegistry
    {
        Dictionary<string,IFunctionObject> functions = new Dictionary<string,IFunctionObject>();
        private FunctionObjectRegistry()
        {
        }

        public static FunctionObjectRegistry Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly FunctionObjectRegistry instance = new FunctionObjectRegistry();
        }

        public bool Register(IFunctionObject functionObject)
        {
            if( null == functionObject)
            {
                Console.WriteLine("Parameter <functionObject> is null!");
                return false;
            }

            if( string.IsNullOrEmpty(functionObject.Name) ) 
            {
                Console.WriteLine("FunctionObject should be neither zero nor empty!");
                return false;
            }

            try
            {
                if ( true == functions.ContainsKey(functionObject.Name) )
                {
                    Console.WriteLine("Function already registered: " +functionObject.Name);
                    return false;
                }

                functions.Add(functionObject.Name, functionObject);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                return false;
            }

            return true;
        }

        public void Clear()
        {
            functions.Clear();
        }

        public IFunctionObject GetFunctionObject(string name)
        {
            if( true == string.IsNullOrEmpty(name) )
            {
                Console.WriteLine("Name should be neither zero nor empty!");
                return null; 
            }

            if( false == functions.ContainsKey(name) ) 
            {
                Console.WriteLine("FunctionObject not registered! [name] = " + name);
                return null; 
            }

            Console.WriteLine("FunctionObject found! [name] = " + name);
            return functions[name];
        }

        public int Count() 
        { 
            return functions.Count; 
        }

        public IFunctionObject RemoveFunctionObject(string name)
        {
            if (true == string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Name should be neither zero nor empty!");
                return null;
            }

            IFunctionObject functionObject = GetFunctionObject(name);
            if (functionObject == null)
            { 
                return null; 
            }

            functions.Remove(name);
            return functionObject;
        }

        public Dictionary<string, IFunctionObject> GetAll()
        { 
            return functions; 
        }
    }
}
