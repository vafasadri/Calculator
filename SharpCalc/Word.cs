using SharpCalc.DataModels;

namespace SharpCalc
{
    public interface INamed
    {
        string Name { get; }
    }
    public interface IValued
    {
        Word? Value { get; }
    }
    /// <summary>
    /// Works just like <see cref="object"/> inside this calculator.
    /// <para> numbers,variables,functions and etc are all considered words</para>
    /// </summary>
    public interface Word
    {
        /// <summary>
        /// Name of the deriving type, mostly used in exceptions like: 
        /// <code>exception: function expected, number provided</code>
        
        /// </summary>
        string TypeName { get; }
        /// <summary>
        /// when overrided in a deriving class, converts the word to a simple representation like its name
        /// <br/> 
        /// </summary>     
        string ToText();
        // Used to print the word with details
        virtual string Print()
        {
            return ToText();
        }
        // used to solve equations
        internal void FindX(VariableLocator locator);
        /// <summary>
        /// when overrided in a deriving class,tries to simplify the word,example:
        ///  <code>new Add(new Number(2),new Number(2))</code> is simplified into <code>new Number(4)</code>
        /// </summary>
        /// <param name="output"></param>
        /// <returns>simplified word / null if could not simplify</returns>
        internal Word? Simplify();
    }
    public interface IContent : Word
    {
        internal Word Derivative(Proxy x);
    }
}
