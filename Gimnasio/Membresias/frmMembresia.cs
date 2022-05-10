using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gimnasio.Membresias
{
    public partial class frmMembresia : Form
    {
        public int idMembresia = 0;
        clsMembresia oMembresia = new clsMembresia();
        public frmMembresia()
        {
            InitializeComponent();
        }

        private void frmMembresia_Load(object sender, EventArgs e)
        {
            cboMeses.SelectedIndex = 0;

            if (idMembresia > 0)
            {
                cargaDatos();
            }
        }

        private void cargaDatos()
        {
            if (oMembresia.getDatos(idMembresia))
            {
                string connStr = "server=localhost;User Id=root;Persist Security Info=True;database=gym";
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = $"Select * from `membresia` where `idMembresia` = '{idMembresia}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtNombre.Text = reader.GetString(1);
                    txtPrecio.Text = reader.GetString(4);
                    cboMeses.Text = reader.GetString(6);
                    dpInicio.Text = reader.GetString(7);
                    dpFinal.Text = reader.GetString(8);
                    initialDate.Value = reader.GetDateTime(9);
                    finalDate.Value = reader.GetDateTime(10);
                }


                //txtNombre.Text = oMembresia.datos.Nombre;
                //txtPrecio.Text = oMembresia.datos.Precio.ToString();
                //cboMeses.Text = oMembresia.datos.meses.ToString();
                //dpInicio.Text = oMembresia.datos.horaInicio.ToString();
                //dpFinal.Text = oMembresia.datos.horaFinal.ToString();
            }
            else
            {
                MessageBox.Show("Ocurrio un problema al cargar los datos " + oMembresia.getError());
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (idMembresia <= 0)
            {
                agrega();
            }
            else
            {
                modifica();
            }
        }

        private void agrega()
        {
             try
            {
                //validaciones
                if (txtNombre.Text.Trim().Equals("") || txtPrecio.Text.Trim().Equals("") || cboMeses.Text.Equals(""))
                {
                    MessageBox.Show("Nombre, Precio y Meses son obligatorios");
                    return;
                }
                if (!ExpresionesRegulares.RegEX.isDecimal(txtPrecio.Text.Trim()))
                {
                    MessageBox.Show("El precio debe ser un numero valido, no se permiten letras ni caracteres que no sean numeros");
                    return;
                }

                //asignacion de datos
                oMembresia.Nombre = txtNombre.Text.Trim();
                oMembresia.Precio = decimal.Parse(txtPrecio.Text.Trim());
                oMembresia.meses = int.Parse(cboMeses.Text.Trim());
                oMembresia.horaInicio = dpInicio.Value.TimeOfDay;
                oMembresia.horaFinal = dpFinal.Value.TimeOfDay;
                oMembresia.idUsuarioLog = Utilidades.clsUsuario.idUsuario;

                string connStr = "server=localhost;User Id=root;Persist Security Info=True;database=gym";
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = $"INSERT INTO `membresia` (`Nombre`,`Precio`,`meses`, `horaInicio`, `horaFinal`,`InitialDate`,`FinalDate`,`idUsuarioCreo`,`idEstado`, `fechaCreacion`) VALUES ('{txtNombre.Text.Trim()}','{decimal.Parse(txtPrecio.Text.Trim())}','{int.Parse(cboMeses.Text.Trim())}','{dpInicio.Value.TimeOfDay}','{dpFinal.Value.TimeOfDay}','{initialDate.Value.ToString("yyyy-MM-dd h:mm tt")}','{finalDate.Value.ToString("yyyy-MM-dd h:mm tt")}', '{Utilidades.clsUsuario.idUsuario}','1','{DateTime.Now.ToString("yyyy-MM-dd h:mm tt")}')";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                this.Close();


                //if (oMembresia.add())
                //{
                //    MessageBox.Show("Registro agregado con exito");
                //    this.Close();
                //}
                //else
                //    MessageBox.Show(oMembresia.getError());

            }
              catch (Exception EX)
              {
                  MessageBox.Show("Ocurrio un error de sistema " + EX.Message);
              }
        }

        private void modifica()
        {

            try
            {
                //validaciones
                if (txtNombre.Text.Trim().Equals("") || txtPrecio.Text.Trim().Equals("") || cboMeses.Text.Equals(""))
                {
                    MessageBox.Show("Nombre, Precio y Meses son obligatorios");
                    return;
                }
                if (!ExpresionesRegulares.RegEX.isDecimal(txtPrecio.Text.Trim()))
                {
                    MessageBox.Show("El precio debe ser un numero valido, no se permiten letras ni caracteres que no sean numeros");
                    return;
                }

                //asignacion de datos
                oMembresia.Nombre = txtNombre.Text.Trim();
                oMembresia.Precio = decimal.Parse(txtPrecio.Text.Trim());
                oMembresia.meses = int.Parse(cboMeses.Text.Trim());
                oMembresia.horaInicio= dpInicio.Value.TimeOfDay;
                oMembresia.horaFinal = dpFinal.Value.TimeOfDay;
                oMembresia.idUsuarioLog = Utilidades.clsUsuario.idUsuario;

                string connStr = "server=localhost;User Id=root;Persist Security Info=True;database=gym";
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                string sql = $"UPDATE `membresia` set `Nombre` = '{txtNombre.Text.Trim()}', `Precio` = '{decimal.Parse(txtPrecio.Text.Trim())}', `meses` = '{int.Parse(cboMeses.Text.Trim())}', `horaInicio` = '{dpInicio.Value.TimeOfDay}',  `horaFinal` = '{dpFinal.Value.TimeOfDay}' ,`InitialDate` = '{initialDate.Value.ToString("yyyy-MM-dd h:mm tt")}',`FinalDate` = '{finalDate.Value.ToString("yyyy-MM-dd h:mm tt")}' where `idMembresia` = '{idMembresia}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();

                this.Close();
                //if (oMembresia.edit(idMembresia))
                //{
                //    MessageBox.Show("Registro modificado con exito");
                //    this.Close();
                //}
                //else
                //    MessageBox.Show(oMembresia.getError());

            }
            catch (Exception EX)
            {
                MessageBox.Show("Ocurrio un error de sistema "+EX.Message);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
