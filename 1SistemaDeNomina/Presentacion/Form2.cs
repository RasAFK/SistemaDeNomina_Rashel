using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _1SistemaDeNomina.Core;
using _1SistemaDeNomina.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TextBox = System.Windows.Forms.TextBox;

namespace _1SistemaDeNomina.Presentacion
{
    public partial class Form2 : Form
    {
        private Repositorio_de_Empleados repositorio = new Repositorio_de_Empleados();

        public Form2()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Controls.OfType<TextBox>().ToList().ForEach(t => t.ReadOnly = true);
        }

        #region METODO Limpiar campos
        private void clearForm()
        {
            this.Controls.OfType<TextBox>().ToList().ForEach(t => t.Text = string.Empty);
        }
        #endregion

        #region METODOS para mostrar empleados en la lista
        private void mostrarEmpleadosEnLista()
        {
            listView1.Items.Clear();
            var datos = Repositorio_de_Empleados.EmpleadosLista.Select(empleado => new ListViewItem(new[]
            {
             empleado.primerNombre,
             empleado.apellidoPaterno,
             empleado.numeroSeguroSocial.ToString(),
              ObtenerDato(empleado as Empleado_Asalariado, e => e.salarioSemanal),
              ObtenerDato(empleado as Empleado_por_Horas, e => e.sueldoPorHora),
              ObtenerDato(empleado as Empleado_por_Horas, e => e.horasTrabajadas),
              ObtenerDato(empleado as Empleado_por_Comision, e => e.ventasBrutas, empleado as Empleado_Asalariado_por_Comision),
              ObtenerDato(empleado as Empleado_por_Comision, e => e.tarifaComision, empleado as Empleado_Asalariado_por_Comision),
              ObtenerDato(empleado as Empleado_Asalariado_por_Comision, e => e.salarioBase),
             empleado.calcularPagoSemanal().ToString("C")
             }));
            listView1.Items.AddRange(datos.ToArray());
        }

        private string ObtenerDato<T>(T empleado, Func<T, decimal> selector, Empleados extraEmpleado = null) where T : Empleados
        {
            return empleado != null ? selector(empleado).ToString(): extraEmpleado is T extra ? selector(extra).ToString()  : "N/A";
        }
        #endregion

        #region Seleccionar de la lista
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = listView1.SelectedItems[0];
            if (!int.TryParse(item.SubItems[2].Text, out int ssn)) return;

            var empleado = Repositorio_de_Empleados.EmpleadosLista
               .FirstOrDefault(emp => emp.numeroSeguroSocial == ssn);
            if (empleado == null) return;

            txtNombre.Text = empleado.primerNombre;
            txtapellidoPaterno.Text = empleado.apellidoPaterno;
            txtnumeroSeguroSocial.Text = empleado.numeroSeguroSocial.ToString();
            if (empleado is Empleado_Asalariado ea)
            {
                comboBox1.SelectedItem = "Empleado Asalariado";
                txtsalarioSemanal.Text = ea.salarioSemanal.ToString();
            }
            else if (empleado is Empleado_por_Horas eph)
            {
                comboBox1.SelectedItem = "Empleado por Horas";
                txtsueldoPorHora.Text = eph.sueldoPorHora.ToString();
                txthorasTrabajadas.Text = eph.horasTrabajadas.ToString();
            }
            else if (empleado is Empleado_por_Comision epc)
            {
                comboBox1.SelectedItem = "Empleado por Comisión";
                txtventasBrutas.Text = epc.ventasBrutas.ToString();
                txttarifaComision.Text = epc.tarifaComision.ToString();
            }
            else if (empleado is Empleado_Asalariado_por_Comision eac)
            {
                comboBox1.SelectedItem = "Empleado Asalariado por Comisión";
                txtventasBrutas.Text = eac.ventasBrutas.ToString();
                txttarifaComision.Text = eac.tarifaComision.ToString();
                txtsalarioBase.Text = eac.salarioBase.ToString();
            }
        }
        #endregion



        #region BOTON Agregar
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int nss;
            if (!int.TryParse(txtnumeroSeguroSocial.Text, out nss))
            {
                MessageBox.Show("Revisar dato en: NUmero de seguro social");
                return;
            }

            string seleccion = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "";
            if (string.IsNullOrEmpty(seleccion))
            {
                MessageBox.Show("Seleccione un tipo de empleado");
                return;
            }

            Empleados nuevoEmpleado = null;
            if (seleccion == "Empleado Asalariado")
            {
                decimal salarioSemanal;
                if (decimal.TryParse(txtsalarioSemanal.Text, out salarioSemanal))
                    nuevoEmpleado = new Empleado_Asalariado(txtNombre.Text, txtapellidoPaterno.Text, nss, salarioSemanal);
                else  Error("Salario semanal");
            }
            else if (seleccion == "Empleado por Horas")
            {
                decimal sueldoPorHora;
                int horasTrabajadas;
                if (decimal.TryParse(txtsueldoPorHora.Text, out sueldoPorHora) && int.TryParse(txthorasTrabajadas.Text, out horasTrabajadas))
                    nuevoEmpleado = new Empleado_por_Horas(txtNombre.Text, txtapellidoPaterno.Text, nss, sueldoPorHora, horasTrabajadas);
                else
                    Error("Sueldo por Hora, Horas Trabajadas");
            }
            else if (seleccion == "Empleado por Comisión")
            {
                decimal ventasBrutas, tarifaComision;
                if (decimal.TryParse(txtventasBrutas.Text, out ventasBrutas) && decimal.TryParse(txttarifaComision.Text, out tarifaComision))
                    nuevoEmpleado = new Empleado_por_Comision(txtNombre.Text, txtapellidoPaterno.Text, nss, ventasBrutas, tarifaComision);
                else
                    Error("Ventas Brutas, Tarifa de Comisión");
            }
            else if (seleccion == "Empleado Asalariado por Comisión")
            {
                decimal salarioBase, ventasBrutas, tarifaComision;
                if (decimal.TryParse(txtsalarioBase.Text, out salarioBase) && decimal.TryParse(txtventasBrutas.Text, out ventasBrutas) && decimal.TryParse(txttarifaComision.Text, out tarifaComision))
                    nuevoEmpleado = new Empleado_Asalariado_por_Comision(txtNombre.Text, txtapellidoPaterno.Text, nss, ventasBrutas, tarifaComision, salarioBase);
                else
                    Error("Salario Base, Ventas Brutas, Tarifa de Comisión");
            }

            if (nuevoEmpleado == null) return;
            repositorio.agregarEmpleado(nuevoEmpleado);
            MessageBox.Show("Empleado agregado");
            mostrarEmpleadosEnLista();
            clearForm();
        }

        private static void Error(string campo)
        {
         MessageBox.Show("Revisar dato en: " + campo);
        }
        #endregion

        #region BOTON Editar o Actualizar en si
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem itemSeleccionado = listView1.SelectedItems[0];
                int numeroSeguroSocial;

                if (int.TryParse(itemSeleccionado.SubItems[2].Text, out numeroSeguroSocial))
                {
                    string nombre = txtNombre.Text;
                    string apellido = txtapellidoPaterno.Text;
                    string seleccion = comboBox1.SelectedItem?.ToString();
                    Empleados nuevoEmpleado = null;

                    switch (seleccion)
                    {
                        case "Empleado Asalariado":
                            decimal salarioSemanal;
                            if (!decimal.TryParse(txtsalarioSemanal.Text, out salarioSemanal))
                            {
                                MessageBox.Show("Revisar datos incertados en: salario semanal");
                                return;
                            }
                            nuevoEmpleado = new Empleado_Asalariado(nombre, apellido, numeroSeguroSocial, salarioSemanal);
                            break;

                        case "Empleado por Horas":
                            decimal sueldoPorHora;
                            int horasTrabajadas;
                            if (!decimal.TryParse(txtsueldoPorHora.Text, out sueldoPorHora) || !int.TryParse(txthorasTrabajadas.Text, out horasTrabajadas))
                            {
                                MessageBox.Show("Revisar datos incertados en: Sueldo por Hora, Horas Trabajadas ");
                                return;
                            }
                            nuevoEmpleado = new Empleado_por_Horas(nombre, apellido, numeroSeguroSocial, sueldoPorHora, horasTrabajadas);
                            break;

                        case "Empleado por Comisión":
                            decimal ventasBrutas, tarifaComision;
                            if (!decimal.TryParse(txtventasBrutas.Text, out ventasBrutas) || !decimal.TryParse(txttarifaComision.Text, out tarifaComision))
                            {
                                MessageBox.Show("Revisar datos incertados en: Ventas Brutas, Tarifa de Comisión");
                                return;
                            }
                            nuevoEmpleado = new Empleado_por_Comision(nombre, apellido, numeroSeguroSocial, ventasBrutas, tarifaComision);
                            break;

                        case "Empleado Asalariado por Comisión":
                            decimal salarioBase;
                            if (!decimal.TryParse(txtsalarioBase.Text, out salarioBase) || !decimal.TryParse(txtventasBrutas.Text, out ventasBrutas) || !decimal.TryParse(txttarifaComision.Text, out tarifaComision))
                            {
                                return;
                            }
                            nuevoEmpleado = new Empleado_Asalariado_por_Comision(nombre, apellido, numeroSeguroSocial, ventasBrutas, tarifaComision, salarioBase);
                            break;

                        default:
                            MessageBox.Show("Seleccione un tipo de empleado");
                            return;
                    }
                    if (nuevoEmpleado != null)
                    {
                        repositorio.modificarEmpleado(numeroSeguroSocial, nuevoEmpleado);
                        MessageBox.Show("Empleado actualizado");
                        mostrarEmpleadosEnLista();
                        clearForm();
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un empleado");
                }
            }
            else
            {
                MessageBox.Show("Seleccione un empleado para editar.");
            }
        }
        #endregion

        #region BOTON Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem itemSeleccionado = listView1.SelectedItems[0];
                int numeroSeguroSocial;

                if (int.TryParse(itemSeleccionado.SubItems[2].Text, out numeroSeguroSocial))
                {
                    repositorio.eliminarEmpleado(numeroSeguroSocial);
                    mostrarEmpleadosEnLista();
                    MessageBox.Show("Empleado eliminado");
                }
                else
                {
                    MessageBox.Show("Seleccione un empleado");
                }
            }
        }
        #endregion

        #region BOTON Reporte
        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            var tipoEmpleado = new Dictionary<Type, string>
            {
                [typeof(Empleado_Asalariado)] = "Asalariado", [typeof(Empleado_por_Horas)] = "Por Horas",
                [typeof(Empleado_por_Comision)] = "Por Comisión", [typeof(Empleado_Asalariado_por_Comision)] = "Asalariado por Comisión"
            };

            var reporte = new StringBuilder()
            .AppendLine("REPORTE DE PAGOS SEMANALES A EMPLEADOS\n")
            .AppendLine($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
            .AppendLine("---------------------------------------------------------------------------------------------")
            .AppendLine("Nombre       | Primer Apellido | Numero Seguro Social | Tipo de Empleado       | Pago Semanal")
            .AppendLine("---------------------------------------------------------------------------------------------");

            Repositorio_de_Empleados.EmpleadosLista.ForEach(empleado =>
            {
            tipoEmpleado.TryGetValue(empleado.GetType(), out string tipoEmpleadoValue);
            reporte.AppendLine($"{empleado.primerNombre,-12} | {empleado.apellidoPaterno,-15} | {empleado.numeroSeguroSocial,-17} | {tipoEmpleadoValue,-25} | {empleado.calcularPagoSemanal():C}");
            });
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Reporte.txt"), reporte.ToString());
            MessageBox.Show($"Reporte generado.\n\nGuardado en la carpeta escritorio");
        }
        #endregion

        #region BOTON Limpiar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
               clearForm();
        }
        #endregion

        #region COMBOBOX
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         clearForm();
         if (comboBox1.SelectedItem == null) return;
         
         string seleccion = comboBox1.SelectedItem.ToString();

         var estados = new Dictionary<string, bool[]>
        {
         { "Empleado Asalariado", new[] { false, false, false, false, true, true, true, true, true } },
         { "Empleado por Horas", new[] { false, false, false, true, false, true, false, true, true } },
         { "Empleado por Comisión", new[] { false, false, false, true, true, false, true, false, true } },
         { "Empleado Asalariado por Comisión", new[] { false, false, false, true, true, false, true, false, false } }
        };
        TextBox[] textBoxes = { txtNombre, txtapellidoPaterno, txtnumeroSeguroSocial, txtsalarioSemanal,txtsueldoPorHora, 
                txttarifaComision, txthorasTrabajadas, txtventasBrutas, txtsalarioBase };
            if (estados.TryGetValue(seleccion, out bool[] estadosTextBox))
            {
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    textBoxes[i].ReadOnly = estadosTextBox[i];
                }
            }
        }
        #endregion

        private void Form2_Load(object sender, EventArgs e)
        {
            string[] items = {"Empleado Asalariado", "Empleado por Horas", "Empleado por Comisión", "Empleado Asalariado por Comisión"};
            comboBox1.Items.AddRange(items);
          
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            repositorio.empleadoEjemplo();
            mostrarEmpleadosEnLista();
        }
    }
}