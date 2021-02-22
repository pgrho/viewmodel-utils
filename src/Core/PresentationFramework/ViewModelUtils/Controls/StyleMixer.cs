using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace Shipwreck.ViewModelUtils.Controls
{
    [ContentProperty(nameof(Styles))]
    public sealed class StyleMixer : MarkupExtension
    {
        public Type TargetType { get; set; }

        private Collection<Style> _Styles;

        public Collection<Style> Styles
        {
            get => _Styles ??= new Collection<Style>();
            set
            {
                if (value != _Styles)
                {
                    _Styles?.Clear();
                    if (value != null)
                    {
                        foreach (var e in value)
                        {
                            _Styles.Add(e);
                        }
                    }
                }
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Styles.Count == 0)
            {
                var st = new Style() { TargetType = TargetType };
                st.Seal();
                return st;
            }
            else if (Styles.Count == 1 && Styles[0].TargetType == TargetType)
            {
                return Styles[0];
            }
            else
            {
                var newStyle = new Style
                {
                    BasedOn = Styles[0],
                    TargetType = TargetType
                };

                foreach (var sourceStyle in Styles.Skip(1))
                {
                    if (sourceStyle.Resources.Count > 0)
                    {
                        newStyle.Resources.MergedDictionaries.Add(sourceStyle.Resources);
                    }

                    CopySetters(sourceStyle.Setters, newStyle.Setters);

                    foreach (var sourceSetter in sourceStyle.Setters)
                    {
                        if (sourceSetter is Setter setter)
                        {
                            newStyle.Setters.Add(new Setter
                            {
                                Property = setter.Property,
                                Value = setter.Value
                            });
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("{0} is not supported.", sourceSetter.GetType().FullName));
                        }
                    }

                    foreach (var sourceTrigger in sourceStyle.Triggers)
                    {
                        TriggerBase newTrigger;
                        if (sourceTrigger is Trigger trigger)
                        {
                            var t = new Trigger
                            {
                                SourceName = trigger.SourceName,
                                Property = trigger.Property,
                                Value = trigger.Value
                            };
                            CopySetters(trigger.Setters, t.Setters);
                            newTrigger = t;
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("{0} is not supported.", sourceTrigger.GetType().FullName));
                        }

                        // TODO: EnterActions
                        if (trigger.EnterActions.Any())
                        {
                            throw new NotSupportedException(string.Format("{0} is not supported.", nameof(trigger.EnterActions)));
                        }

                        // TODO: ExitActions
                        if (trigger.EnterActions.Any())
                        {
                            throw new NotSupportedException(string.Format("{0} is not supported.", nameof(trigger.EnterActions)));
                        }

                        newStyle.Triggers.Add(newTrigger);
                    }
                }
                newStyle.Seal();

                return newStyle;
            }
        }

        private static void CopySetters(SetterBaseCollection source, SetterBaseCollection destination)
        {
            foreach (var bs in source)
            {
                if (bs is Setter setter)
                {
                    destination.Add(new Setter
                    {
                        TargetName = setter.TargetName,
                        Property = setter.Property,
                        Value = setter.Value
                    });
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}
