using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Threading;

namespace ConCurrencyCalculator
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly HttpClient _client;

        private double _dkk;
        public double DKK { get => _dkk; set => SetProperty(ref _dkk, value); }

        private double _usd;
        public double USD { get => _usd; set => SetProperty(ref _usd, value); }

        private double _gbp;
        public double GBP { get => _gbp; set => SetProperty(ref _gbp, value); }

        private double _eur;
        public double EUR { get => _eur; set => SetProperty(ref _eur, value); }

        private long _timer;
        public long Timer { get => _timer; set => SetProperty(ref _timer, value); }

        private readonly Stopwatch _stopWatch;

        private readonly DispatcherTimer _dispatcherTimer;

        public MainWindowViewModel()
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<App>().Build();
            var apiKey = configuration["ApiKey"];

            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.apilayer.com/")
            };
            _client.DefaultRequestHeaders.Add("apikey", apiKey);

            _stopWatch = new Stopwatch();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += (s, e) => Timer = _stopWatch.ElapsedMilliseconds;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);
            _dispatcherTimer.Start();
        }

        public ICommand Calculate => new Command(async _ => await CalculateRates());

        private async Task CalculateRates()
        {
            _stopWatch.Restart();

            USD = await GetRate("DKK", "USD", DKK);
            GBP = await GetRate("DKK", "GBP", DKK);
            EUR = await GetRate("DKK", "EUR", DKK);

            _stopWatch.Stop();
        }

        private async Task<double> GetRate(string from, string to, double amount)
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            var path = $"currency_data/convert?to={to}&from={from}&amount={amount}";

            var json = await _client.GetStringAsync(path);

            dynamic obj = JsonConvert.DeserializeObject(json)!;

            return obj.result;
        }
    }
}
