namespace AppGymS4.Views;

public partial class vRegistro : ContentPage
{
    private const decimal CostoMembresia = 1200m;
    private const decimal PorcentajeMinimo = 0.10m;
    private const decimal PorcentajeMaximo = 0.50m;
    private const int NumeroCuotas = 5;
    private const decimal PorcentajeRecargo = 0.02m;
    private const decimal CostoCasillero = 10m;
    private const int EdadMinima = 16;

    private readonly string _usuario;

    public vRegistro(string usuario)
    {
        InitializeComponent();
        _usuario = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {usuario}";
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
    }

    private async void btnCalcular_Clicked(object? sender, EventArgs e)
    {
        if (!TryObtenerMontoInicial(out decimal montoInicial))
        {
            await DisplayAlertAsync("Error",
                $"El monto inicial debe estar entre {CostoMembresia * PorcentajeMinimo} y {CostoMembresia * PorcentajeMaximo}", "OK");
            return;
        }

        txtPagoMensual.Text = CalcularPagoMensual(montoInicial, swCasillero.IsToggled).ToString("F2");
    }

    // Recalcula al cambiar el casillero, solo si el monto inicial ya es válido
    private void swCasillero_Toggled(object? sender, ToggledEventArgs e)
    {
        txtPagoMensual.Text = TryObtenerMontoInicial(out decimal montoInicial)
            ? CalcularPagoMensual(montoInicial, e.Value).ToString("F2")
            : "";
    }

    private async void btnVerResumen_Clicked(object? sender, EventArgs e)
    {
        string? error = ValidarFormulario(out int edad, out decimal montoInicial);
        if (error != null)
        {
            await DisplayAlertAsync("Error", error, "OK");
            return;
        }

        bool casillero = swCasillero.IsToggled;
        decimal pagoMensual = CalcularPagoMensual(montoInicial, casillero);
        decimal pagoTotal = montoInicial + pagoMensual * NumeroCuotas;
        txtPagoMensual.Text = pagoMensual.ToString("F2");

        await Navigation.PushAsync(new vResumen(
            _usuario,
            txtNombre.Text.Trim(),
            txtApellido.Text.Trim(),
            edad,
            dpFechaInicio.Date ?? DateTime.Today,
            pkPlan.SelectedItem.ToString()!,
            casillero,
            montoInicial,
            pagoMensual,
            pagoTotal));

        LimpiarCampos();
    }

    // Cuota = (costo - inicial) / 5 + 2 % del costo (+ 10 si tiene casillero)
    private static decimal CalcularPagoMensual(decimal montoInicial, bool casillero)
    {
        decimal saldo = CostoMembresia - montoInicial;
        decimal cuota = saldo / NumeroCuotas + CostoMembresia * PorcentajeRecargo;
        if (casillero)
            cuota += CostoCasillero;
        return cuota;
    }

    private bool TryObtenerMontoInicial(out decimal montoInicial)
    {
        return decimal.TryParse(txtMontoInicial.Text, out montoInicial)
               && montoInicial >= CostoMembresia * PorcentajeMinimo
               && montoInicial <= CostoMembresia * PorcentajeMaximo;
    }

    private string? ValidarFormulario(out int edad, out decimal montoInicial)
    {
        edad = 0;
        montoInicial = 0;

        if (string.IsNullOrWhiteSpace(txtNombre.Text))
            return "Ingrese el nombre";
        if (string.IsNullOrWhiteSpace(txtApellido.Text))
            return "Ingrese el apellido";
        if (!int.TryParse(txtEdad.Text, out edad))
            return "La edad debe ser un número entero";
        if (edad < EdadMinima)
            return $"La edad mínima es {EdadMinima} años";
        if (dpFechaInicio.Date is null || dpFechaInicio.Date.Value.Date < DateTime.Today)
            return "La fecha de inicio no puede ser una fecha pasada";
        if (pkPlan.SelectedIndex < 0)
            return "Seleccione un plan";
        if (!TryObtenerMontoInicial(out montoInicial))
            return $"El monto inicial debe estar entre {CostoMembresia * PorcentajeMinimo} y {CostoMembresia * PorcentajeMaximo}";

        return null;
    }

    private void LimpiarCampos()
    {
        txtNombre.Text = "";
        txtApellido.Text = "";
        txtEdad.Text = "";
        dpFechaInicio.Date = DateTime.Today;
        pkPlan.SelectedIndex = -1;
        swCasillero.IsToggled = false;
        txtMontoInicial.Text = "";
        txtPagoMensual.Text = "";
    }
}
