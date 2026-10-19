using CajaArequipa.Helpers;
using Microsoft.Maui.Controls;

namespace CajaArequipa.UI
{

    public class EntradaTipadaBehavior : Behavior<Entry>
    {
        public static readonly BindableProperty TipoProperty =
            BindableProperty.Create(nameof(Tipo), typeof(TipoEntrada), typeof(EntradaTipadaBehavior), TipoEntrada.Texto);

        public static readonly BindableProperty MaxLongitudProperty =
            BindableProperty.Create(nameof(MaxLongitud), typeof(int), typeof(EntradaTipadaBehavior), 100);

        public static readonly BindableProperty MaxDecimalesProperty =
            BindableProperty.Create(nameof(MaxDecimales), typeof(int), typeof(EntradaTipadaBehavior), 2);

        public TipoEntrada Tipo
        {
            get { return (TipoEntrada)GetValue(TipoProperty); }
            set { SetValue(TipoProperty, value); }
        }

        public int MaxLongitud
        {
            get { return (int)GetValue(MaxLongitudProperty); }
            set { SetValue(MaxLongitudProperty, value); }
        }

        public int MaxDecimales
        {
            get { return (int)GetValue(MaxDecimalesProperty); }
            set { SetValue(MaxDecimalesProperty, value); }
        }

        private bool ajustando;

        protected override void OnAttachedTo(Entry entry)
        {
            base.OnAttachedTo(entry);

            if (Tipo == TipoEntrada.Entero || Tipo == TipoEntrada.Decimal)
            {
                entry.Keyboard = Keyboard.Numeric;
            }

            entry.TextChanged += AlCambiarTexto;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= AlCambiarTexto;
            base.OnDetachingFrom(entry);
        }

        private void AlCambiarTexto(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;

            if (ajustando || entry == null)
            {
                return;
            }

            string nuevo = e.NewTextValue ?? "";
            string filtrado = InputHelper.Filtrar(nuevo, Tipo, MaxLongitud, MaxDecimales);

            if (filtrado == nuevo)
            {
                return;
            }

            ajustando = true;
            try
            {
                entry.Text = filtrado;
                entry.CursorPosition = filtrado.Length;
            }
            finally
            {
                ajustando = false;
            }
        }
    }
}
