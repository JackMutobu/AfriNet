using Blazored.LocalStorage;
using System.Text.Json;

namespace AfriNetCustomerApp.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        public event EventHandler<ChangingEventArgs> Changing;
        public event EventHandler<ChangedEventArgs> Changed;

        public ValueTask ClearAsync(CancellationToken cancellationToken = default)
        {
            Preferences.Default.Clear();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(Preferences.Default.ContainsKey(key));
        }

        public ValueTask<string> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(Preferences.Default.Get(key, string.Empty));
        }

        public async ValueTask<T> GetItemAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var itemString = await GetItemAsStringAsync(key);
            if(string.IsNullOrEmpty(itemString))
                return default(T);
            var item = JsonSerializer.Deserialize<T>(itemString);
            return item;
        }

        public ValueTask<string> KeyAsync(int index, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default)
        {
            Preferences.Default.Remove(key);
            return ValueTask.CompletedTask;
        }

        public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
                Preferences.Default.Remove(key);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default)
        {
            Preferences.Default.Set(key, data);
            return ValueTask.CompletedTask;
        }

        public async ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            var itemString = JsonSerializer.Serialize(data);
            await SetItemAsStringAsync(key, itemString);
        }
    }
}
