namespace Zenox.Wpf.Core.Common.MVVM.FactoryInjection
{
    public interface IAppObject
    {
        AppKontext Kontext { get; set; }

        void OnFehlerAufgetreten(
                        FehlerAufgetretenEventArgs e);

        void StartMelden([System.Runtime.CompilerServices.CallerMemberName] string aufrufer = null!);

        void EndeMelden([System.Runtime.CompilerServices.CallerMemberName] string aufrufer = null!);
    }
}
