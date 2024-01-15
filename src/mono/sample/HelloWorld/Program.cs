using System;
using System.Collections.Generic;
using System.Reflection;

namespace HelloWorld
{
    public sealed class AttributeWithIntArrayArgument : Attribute
    {
        public AttributeWithIntArrayArgument(params int[] ints)
        {
        }
    }

    [AttributeWithIntArrayArgument(1, 2, 3)]
    public interface IHasAttributeWithIntArray
    {
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Type targetType = typeof(IHasAttributeWithIntArray);

            var attributes = targetType.CustomAttributes;
            foreach (var attribute in attributes)
            {
                var typedArgs = attribute.ConstructorArguments;
                int i = 0;
                foreach (var typedArg in typedArgs)
                {
                    Console.WriteLine($"typedArgs{i}.ArgumentType = {typedArg.ArgumentType}");
                    Console.WriteLine($"typedArgs{i}.Value = {typedArg.Value}");
                    var argElems = (System.Reflection.CustomAttributeTypedArgument[])typedArg.Value;
                    int j = 0;
                    foreach (var argElem in argElems)
                    {
                        Console.WriteLine($"argElems{j}.ArgumentType = {argElem.ArgumentType}");
                        Console.WriteLine($"argElems{j}.Value = {argElem.Value}");
                        j++;
                    }
                }
            }
        }
    }
}