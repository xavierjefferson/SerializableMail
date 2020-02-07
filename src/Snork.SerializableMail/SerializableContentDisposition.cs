using System;
using System.Net.Mime;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableContentDisposition
    {
        private SerializableStringDictionary _parameters;

        public SerializableContentDisposition()
        {
            CopyProperties(new ContentDisposition(), this);
        }

        public SerializableContentDisposition(string contentDisposition)
        {
            CopyProperties(new ContentDisposition(contentDisposition), this);
        }

        public SerializableStringDictionary Parameters
        {
            get => _parameters;
            set => _parameters = value ?? throw new ArgumentNullException(nameof(Parameters));
        }

        public string FileName { get; set; }
        public string DispositionType { get; set; }
        public bool Inline { get; set; }
        public string Boundary { get; set; }
        public string MediaDisposition { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long Size { get; set; }
        public DateTime ReadDate { get; set; }

        /// <summary>Returns a string representation of this <see cref="T:Snork.SerializableMail.SerializableContentDisposition" /> object.</summary>
        /// <returns>
        ///     A <see cref="T:System.String" /> that contains the current settings for this
        ///     <see cref="T:Snork.SerializableMail.SerializableContentDisposition" />.
        /// </returns>
        public override string ToString()
        {
            return ((ContentDisposition) this).ToString();
        }

        internal static ContentDisposition CopyProperties(SerializableContentDisposition source,
            ContentDisposition destination)
        {
            destination.CreationDate = source.CreationDate;
            destination.DispositionType = source.DispositionType;
            destination.FileName = source.FileName;
            destination.Inline = source.Inline;
            destination.ModificationDate = source.ModificationDate;
            source.Parameters.CopyKeyValuePairs(destination.Parameters);
            destination.ReadDate = source.ReadDate;
            destination.Size = source.Size;
            return destination;
        }

        public static implicit operator ContentDisposition(
            SerializableContentDisposition serializableContentDisposition)
        {
            return CopyProperties(serializableContentDisposition, new ContentDisposition());
        }

        internal static SerializableContentDisposition CopyProperties(ContentDisposition source,
            SerializableContentDisposition destination)
        {
            destination.CreationDate = source.CreationDate;
            destination.DispositionType = source.DispositionType;
            destination.FileName = source.FileName;
            destination.Inline = source.Inline;
            destination.ModificationDate = source.ModificationDate;
            destination.Parameters = source.Parameters;
            destination.ReadDate = source.ReadDate;
            destination.Size = source.Size;
            return destination;
        }

        public static implicit operator SerializableContentDisposition(ContentDisposition contentDisposition)
        {
            return CopyProperties(contentDisposition, new SerializableContentDisposition());
        }
    }
}