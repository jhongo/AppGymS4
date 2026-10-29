using System.Text;

namespace AppGymS4.Views;

public partial class vResumen : ContentPage
{
    private readonly string _usuario;

    public vResumen(string usuario, string nombre, string apellido, int edad, DateTime fechaInicio,
        string plan, bool casillero, decimal montoInicial, decimal pagoMensual, decimal pagoTotal)
    {
        InitializeComponent();
        _usuario = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {usuario}";
        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad.ToString();
        lblFechaInicio.Text = fechaInicio.ToString("dd/MM/yyyy");
        lblPlan.Text = plan;
        lblCasillero.Text = casillero ? "Sí" : "No";
        lblMontoInicial.Text = montoInicial.ToString("F2");
        lblPagoMensual.Text = pagoMensual.ToString("F2");
        lblPagoTotal.Text = pagoTotal.ToString("F2");
    }

    private async void btnExportarResumen_Clicked(object? sender, EventArgs e)
    {
        try
        {
            string nombreArchivo = $"resumen_{lblNombre.Text}_{lblApellido.Text}_{DateTime.Now:yyyyMMddHHmmss}.txt";
            string ruta = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);
            await File.WriteAllTextAsync(ruta, GenerarTexto());

            await DisplayAlertAsync("Exportar resumen", $"Archivo generado:\n{ruta}", "OK");
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Resumen del socio",
                File = new ShareFile(ruta)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"No se pudo exportar el resumen: {ex.Message}", "OK");
        }
    }

    private string GenerarTexto()
    {
        var sb = new StringBuilder();
        sb.AppendLine("RESUMEN");
        sb.AppendLine($"Usuario conectado: {_usuario}");
        sb.AppendLine($"Fecha de exportación: {DateTime.Now:dd/MM/yyyy HH:mm}");
        sb.AppendLine();
        sb.AppendLine($"Nombre: {lblNombre.Text}");
        sb.AppendLine($"Apellido: {lblApellido.Text}");
        sb.AppendLine($"Edad: {lblEdad.Text}");
        sb.AppendLine($"Fecha de inicio: {lblFechaInicio.Text}");
        sb.AppendLine($"Plan: {lblPlan.Text}");
        sb.AppendLine($"Casillero: {lblCasillero.Text}");
        sb.AppendLine($"Monto Inicial: {lblMontoInicial.Text}");
        sb.AppendLine($"Pago mensual: {lblPagoMensual.Text}");
        sb.AppendLine($"Pago Total: {lblPagoTotal.Text}");
        return sb.ToString();
    }

    private async void btnCerrarSesion_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
