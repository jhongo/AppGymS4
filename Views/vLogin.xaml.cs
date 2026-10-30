namespace AppGymS4.Views;

public partial class vLogin : ContentPage
{
    // Matriz 4 x 2: [usuario, contraseña]
    private static readonly string[,] Usuarios =
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnIniciarSesion_Clicked(object? sender, EventArgs e)
    {
        string usuario = txtUsuario.Text?.Trim() ?? "";
        string contrasena = txtContrasena.Text ?? "";

        if (!ValidarCredenciales(usuario, contrasena))
        {
            await DisplayAlertAsync("Error", "Usuario o contraseña incorrectos", "OK");
            return;
        }

        txtUsuario.Text = "";
        txtContrasena.Text = "";
        await Navigation.PushAsync(new vRegistro(usuario));
    }

    private async void btnAcercaDe_Clicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync("Acerca de",
            "Nombre: John Gomez\nCarrera: Ingeniería Informática\nParalelo: A", "OK");
    }

    private static bool ValidarCredenciales(string usuario, string contrasena)
    {
        for (int i = 0; i < Usuarios.GetLength(0); i++)
        {
            if (Usuarios[i, 0] == usuario && Usuarios[i, 1] == contrasena)
                return true;
        }
        return false;
    }
}
