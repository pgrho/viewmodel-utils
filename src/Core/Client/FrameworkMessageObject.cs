using System.Runtime.Serialization;

namespace Shipwreck.ViewModelUtils.Client
{
    [DataContract]
    public abstract class FrameworkMessageObject
    {
        private IMessageObjectCollection _Collection;
        private FrameworkMessageBase _Message;

        protected FrameworkMessageObject(FrameworkMessageBase message)
        {
            _Message = message;
        }

        protected internal IMessageObjectCollection Collection
        {
            get => _Collection;
            set
            {
                var pc = _Collection;
                var pr = Message;
                if (value != pc)
                {
                    _Collection = value;
                    OnCollectionChanged(pc, pr);
                }
            }
        }

        [IgnoreDataMember]
        public FrameworkMessageBase Message
        {
            get
            {
                if (_Message == null)
                {
                    for (var c = Collection; c != null; c = c.Owner?.Collection)
                    {
                        if (c.Message != null)
                        {
                            return _Message = c.Message;
                        }
                    }
                }
                return _Message;
            }
            internal set
            {
                var pc = _Collection;
                var pr = Message;
                if (value != _Message)
                {
                    _Message = value;
                    OnCollectionChanged(pc, pr);
                }
            }
        }

        protected internal virtual void OnCollectionChanged(IMessageObjectCollection previousCollection, FrameworkMessageBase previousResponse)
        {
            if (_Collection != null
                || previousCollection != null)
            {
                _Message = null;
            }
        }
    }
}
