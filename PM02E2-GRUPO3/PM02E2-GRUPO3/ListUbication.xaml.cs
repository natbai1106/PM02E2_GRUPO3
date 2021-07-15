﻿using System;
using SQLite;
using E1201710110129.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace PM02E2_GRUPO3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListUbication : ContentPage
    {
        private Mapa seleccinarId;
        public ListUbication()
        {
             InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conexion = new SQLiteConnection(App.UbicacionDB))
            {   //mandamos a que tabla se guarda
                conexion.CreateTable<Mapa>();

                var listmaps = conexion.Table<Mapa>().ToList();
                ListaUbicacion.ItemsSource = listmaps;

            }
        }

        private async void btnViewMap_Clicked(object sender, EventArgs e)
        {
            var Gps = CrossGeolocator.Current;
            if (Gps.IsGeolocationEnabled)//Servicio de Geolocalizacion existente
            {


                if (Gps.IsGeolocationEnabled)//VALIDA QUE EL GPS ESTA ENCENDIDO
                {

                    

                    using (SQLiteConnection conexion = new SQLiteConnection(App.UbicacionDB))
                    {
                        if (seleccinarId != null)
                        {
                            OnBackButtonPressed();
                            
                        }

                        else
                            messagetSelect();
                    }
                }
               
            }
            else
            {
                await DisplayAlert("GPS Apagado", "Para ver la Ubicacion de manera Correcta. Por favor, Active el GPS/ Ubicacion", "OK");
            }
                
            }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {

            using (SQLiteConnection conexion = new SQLiteConnection(App.UbicacionDB))
            {
                if (seleccinarId != null)
                {
                    await DisplayAlert("Aviso", "Se Eliminara el Campo Seleccionado(ID): " + seleccinarId.Id + " Nombre: " + seleccinarId.DescripcionCorta + " De la lista de Ubicaciones guardadas", "Ok");

                    var ListaMapas = conexion.Delete<Mapa>(seleccinarId.Id);
                    OnAppearing();
                }
                else
                    messagetSelect();
            }

        }

        private void ListaUbicacion_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            seleccinarId=e.SelectedItem as Mapa;


            //Confirmamos que se seleccione
            //DisplayAlert("Informacion de Registro> " + seleccinarId.Id + " " + seleccinarId.DescripcionCorta, " Ubicacion Larga> " + seleccinarId.DescripcionLarga + " Coordenadas >> " + seleccinarId.Latitud + " " + seleccinarId.Longitud, "OK");


        }
        
           private async void messagetSelect()
        {
            await DisplayAlert("Sin Seleccion", "Por Favor Seleccione un Dato", "OK");
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();

            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Accion", "¿Desea ir a la Ubicacion indicada?", "Yes", "No");
                if (result) Ver();

            });


            return true;
        }

        private async void Ver()
        {
           
                    var mapa = new
                    {
                        Id = seleccinarId.Id,
                        Latitud = seleccinarId.Latitud,
                        Longitud = seleccinarId.Longitud,
                        DescripcionCorta = seleccinarId.DescripcionCorta,
                        DescripcionLarga = seleccinarId.DescripcionLarga
                    };

                    //await DisplayAlert("Datos a Enviar> " + seleccinarId.Id + " " + seleccinarId.DescripcionCorta, " Ubicacion Larga> " + seleccinarId.DescripcionLarga + " Coordenadas >> " + seleccinarId.Latitud + " " + seleccinarId.Longitud, "OK");

                    var Page = new MapPage();
                    Page.BindingContext = mapa;
                    await Navigation.PushAsync(Page);
                     seleccinarId = null;

        }
    }
}