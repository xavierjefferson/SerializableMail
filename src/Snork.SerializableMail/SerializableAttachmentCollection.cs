using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;

namespace Snork.SerializableMail
{ /// <summary>Stores attachments to be sent as part of an e-mail message.</summary>
    [Serializable]
    public class SerializableAttachmentCollection : Collection<SerializableAttachment>, IDisposable
    {
        private bool disposed;

        public SerializableAttachmentCollection()
        {
        }

        public SerializableAttachmentCollection(IEnumerable<SerializableAttachment> attachments) : base(
            attachments.ToList())
        {
        } /// <summary>Releases all resources used by the <see cref="T:Snork.SerializableMail.SerializableAttachmentCollection" />. </summary>
        public void Dispose()
        {
            if (this.disposed)
                return;
            foreach (var attachment in this)
                attachment.Dispose();
            this.Clear();
            this.disposed = true;
        }

        protected override void RemoveItem(int index)
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            base.RemoveItem(index);
        }

        protected override void ClearItems()
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            base.ClearItems();
        }

        protected override void SetItem(int index, SerializableAttachment item)
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            base.SetItem(index, item);
        }

        protected override void InsertItem(int index, SerializableAttachment item)
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.GetType().FullName);
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            base.InsertItem(index, item);
        }
    }
}