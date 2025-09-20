using System.Buffers;

namespace RecyclableBuffer
{
    /// <summary>
    /// 表示一个只读序列的分段，内部使用 <see cref="RentedBuffer"/> 作为数据源。
    /// </summary>
    sealed class RentedSegment : ReadOnlySequenceSegment<byte>
    {
        /// <summary>
        /// 使用指定的 <see cref="RentedBuffer"/> 初始化 <see cref="RentedSegment"/> 实例。
        /// </summary>
        /// <param name="buffer">用于分段的数据缓冲区。</param>
        public RentedSegment(RentedBuffer buffer)
        {
            this.Memory = buffer.WritternMemory;
        }

        /// <summary>
        /// 将新的 <see cref="RentedBuffer"/> 追加为下一个分段，并返回新分段。
        /// </summary>
        /// <param name="buffer">要追加的缓冲区。</param>
        /// <returns>新创建的 <see cref="RentedSegment"/> 实例。</returns>
        public RentedSegment Append(RentedBuffer buffer)
        {
            var nextSegment = new RentedSegment(buffer)
            {
                RunningIndex = this.RunningIndex + this.Memory.Length
            };
            this.Next = nextSegment;
            return nextSegment;
        }
    }
}
