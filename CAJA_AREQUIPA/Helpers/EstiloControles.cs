using System;
using Microsoft.Maui.Handlers;

namespace CajaArequipa.Helpers
{
    // Quita el subrayado / borde nativo de Entry y Picker para que se vean como los
    // campos de la interfaz (el borde lo dibuja el Border que los envuelve en el XAML).
    // Es solo un retoque visual: si la plataforma no deja aplicarlo, se ignora el error
    // y la aplicación sigue funcionando con el estilo nativo del control.
    public static class EstiloControles
    {
        public static void Aplicar()
        {
            EntryHandler.Mapper.AppendToMapping("CajaArequipaSinBorde", (handler, vista) =>
            {
                try
                {
#if ANDROID
                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                    PonerTransparente(handler.PlatformView.Resources,
                        "TextControlBackgroundFocused",
                        "TextControlBackgroundPointerOver",
                        "TextControlBorderBrushFocused",
                        "TextControlBorderBrushPointerOver");
#endif
                }
                catch (Exception)
                {
                    // Retoque opcional: no debe impedir que la pantalla se abra.
                }
            });

            PickerHandler.Mapper.AppendToMapping("CajaArequipaSinBorde", (handler, vista) =>
            {
                try
                {
#if ANDROID
                    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif WINDOWS
                    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                    PonerTransparente(handler.PlatformView.Resources,
                        "ComboBoxBackground",
                        "ComboBoxBackgroundPointerOver",
                        "ComboBoxBackgroundFocused",
                        "ComboBoxBackgroundPressed",
                        "ComboBoxBorderBrush",
                        "ComboBoxBorderBrushPointerOver",
                        "ComboBoxBorderBrushFocused",
                        "ComboBoxBorderBrushPressed");
#endif
                }
                catch (Exception)
                {
                    // Retoque opcional: no debe impedir que la pantalla se abra.
                }
            });
        }

#if WINDOWS

        private static void PonerTransparente(Microsoft.UI.Xaml.ResourceDictionary recursos, params string[] claves)
        {
            for (int i = 0; i < claves.Length; i++)
            {
                try
                {
                    recursos[claves[i]] = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
                catch (Exception)
                {
                    // Se conserva el color nativo de esa clave.
                }
            }
        }
#endif
    }
}
