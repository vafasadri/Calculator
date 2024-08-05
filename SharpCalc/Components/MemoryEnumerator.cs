using System.Collections;

namespace SharpCalc.Components
{
    internal struct MemoryEnumerable<T> : IEnumerable<T>
    {
        ReadOnlyMemory<T> _memory;

        public MemoryEnumerable(ReadOnlyMemory<T> memory) => _memory = memory;

        public IEnumerator<T> GetEnumerator() => new MemoryEnumerator<T>(_memory);

        IEnumerator IEnumerable.GetEnumerator() => new MemoryEnumerator<T>(_memory);
    }
    internal struct MemoryEnumerator<T> : IEnumerator<T>
    {
        ReadOnlyMemory<T> _memory;
        int _index;

        public MemoryEnumerator(ReadOnlyMemory<T> memory) => _memory = memory;

        public T Current => _memory.Span[_index];

        object IEnumerator.Current => _memory.Span[_index];

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            _index++;
            return _index < _memory.Length;
        }

        public void Reset()
        {
            _index = 0;
        }
    }

}
