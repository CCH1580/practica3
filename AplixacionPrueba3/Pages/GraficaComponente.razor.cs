

namespace AplixacionPrueba3.Pages;

public partial class GraficaComponente
{
    public List<object> Series1Data = new List<object>() { 5600, 6300, 7400, 9100, 1100, 1300 };
    
    public List<object> Series2Data = new List<object>() { 5200, 3400, 2300, 4800, 6700, 8300 };
    
    public string[] Categories = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio" };
    public string Color { get; set; }
}
