using Microsoft.ML;
using System;
using System.IO;
using WPFBasics.Common;
using WPFBasics.Common.Services;

namespace WPFBasics.Common.ML
{
    /// <summary>
    /// Generischer, dynamischer Service für ML.NET-Modelle zur einfachen Nutzung von Vorhersagen und Training.
    /// </summary>
    /// <typeparam name="TInput">Typ der Eingabedaten (z.B. ein Daten-Record).</typeparam>
    /// <typeparam name="TOutput">Typ der Vorhersageausgabe (z.B. ein Prediction-Record).</typeparam>
    public class MLApiService<TInput, TOutput>
        where TInput : class
        where TOutput : class, new()
    {
        /// <summary>
        /// Logger für Fehler und Informationen.
        /// </summary>
        public LogService Logger { get; init; }

        /// <summary>
        /// MLContext-Instanz für alle ML.NET-Operationen.
        /// </summary>
        public MLContext MLContext { get; init; } = new();

        /// <summary>
        /// Das geladene oder trainierte ML.NET-Modell.
        /// </summary>
        public ITransformer Model { get; private set; }

        /// <summary>
        /// PredictionEngine für Einzelvorhersagen.
        /// </summary>
        public PredictionEngine<TInput, TOutput> PredictionEngine { get; private set; }

        /// <summary>
        /// Lädt ein ML.NET-Modell aus einer Datei.
        /// </summary>
        /// <param name="modelPath">Pfad zur Modelldatei.</param>
        /// <returns>True, wenn das Modell erfolgreich geladen wurde.</returns>
        public bool LoadModel(string modelPath)
        {
            try
            {
                using var stream = File.OpenRead(modelPath);
                Model = MLContext.Model.Load(stream, out _);
                PredictionEngine = MLContext.Model.CreatePredictionEngine<TInput, TOutput>(Model);
                Logger?.Log($"MLApiService: Modell aus '{modelPath}' geladen.", LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "MLApiService.LoadModel");
                Logger?.LogException(ex, "MLApiService.LoadModel");
                return false;
            }
        }

        /// <summary>
        /// Speichert das aktuelle Modell in eine Datei.
        /// </summary>
        /// <param name="modelPath">Pfad zur Zieldatei.</param>
        /// <param name="inputSchema">Schema der Eingabedaten.</param>
        /// <returns>True, wenn das Modell erfolgreich gespeichert wurde.</returns>
        public bool SaveModel(string modelPath, DataViewSchema inputSchema)
        {
            try
            {
                using var stream = File.Create(modelPath);
                MLContext.Model.Save(Model, inputSchema, stream);
                Logger?.Log($"MLApiService: Modell nach '{modelPath}' gespeichert.", LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "MLApiService.SaveModel");
                Logger?.LogException(ex, "MLApiService.SaveModel");
                return false;
            }
        }

        /// <summary>
        /// Trainiert ein Modell mit den angegebenen Trainingsdaten und Pipeline.
        /// </summary>
        /// <param name="data">Trainingsdaten als IDataView.</param>
        /// <param name="pipeline">ML.NET-EstimatorPipeline.</param>
        /// <returns>True, wenn das Training erfolgreich war.</returns>
        public bool TrainModel(IDataView data, IEstimator<ITransformer> pipeline)
        {
            try
            {
                Model = pipeline.Fit(data);
                PredictionEngine = MLContext.Model.CreatePredictionEngine<TInput, TOutput>(Model);
                Logger?.Log("MLApiService: Modell erfolgreich trainiert.", LogLevel.Info);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "MLApiService.TrainModel");
                Logger?.LogException(ex, "MLApiService.TrainModel");
                return false;
            }
        }

        /// <summary>
        /// Führt eine Einzelvorhersage mit dem geladenen Modell durch.
        /// </summary>
        /// <param name="input">Eingabedaten für die Vorhersage.</param>
        /// <returns>Vorhersageergebnis oder null bei Fehler.</returns>
        public TOutput Predict(TInput input)
        {
            try
            {
                if (PredictionEngine == null)
                    throw new InvalidOperationException("PredictionEngine ist nicht initialisiert. Modell laden oder trainieren!");

                return PredictionEngine.Predict(input);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Logger?.LogException(ex, "MLApiService.Predict");
                Logger?.LogException(ex, "MLApiService.Predict");
                return null;
            }
        }
    }
}
