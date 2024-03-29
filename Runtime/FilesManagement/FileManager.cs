using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class FileManager : IFileManager
    {
        private const string PROTOCOL_HEADER_REGEX = @"^\w+://$";

        private readonly Dictionary<string, IProtocol> protocols = new();
        private readonly Regex protocolRegex;
        private readonly Protocol defaultProtocol;

        public FileManager(ISerializer defaultSerializer, IFileSystem defaultFileSystem)
        {
            protocolRegex = new Regex(PROTOCOL_HEADER_REGEX);

            defaultProtocol = new Protocol(defaultSerializer, defaultFileSystem);
        }

        public void RegisterProtocol(string protocolHeader, ISerializer serializer, IFileSystem fileSystem)
        {
            if (serializer == null)
            {
                throw new Exception($"Cannot register protocol with a null {nameof(ISerializer)}");
            }

            if (fileSystem == null)
            {
                throw new Exception($"Cannot register protocol with a null {nameof(IFileSystem)}");
            }

            var protocol = new Protocol(serializer, fileSystem);

            RegisterProtocol(protocolHeader, protocol);
        }

        public void RegisterProtocol(string protocolHeader, IProtocol protocol)
        {
            if (!protocolRegex.IsMatch(protocolHeader ?? string.Empty))
            {
                throw new Exception($"Invalid protocol '{protocolHeader}'. Only word characters allowed.");
            }

            if (protocols.ContainsKey(protocolHeader))
            {
                throw new Exception($"Cannot register protocol {protocolHeader}, it is already registered");
            }

            protocols.Add(protocolHeader, protocol);
        }

        public bool UnregisterProtocol(string protocolHeader)
        {
            if (protocols.ContainsKey(protocolHeader))
            {
                protocols.Remove(protocolHeader);
                return true;
            }

            return false;
        }

        public async Task<bool> Exists(string uri, CancellationToken token = default)
        {
            return await GetProtocol(uri).Exists(uri, token);
        }

        public async Task<File<T>> Read<T>(string uri, string version = null, CancellationToken token = default)
        {
            var value = await GetProtocol(uri).Read<T>(uri, version, token);

            return new File<T>(uri, (T)value);
        }

        public async Task<File<T>> Write<T>(string uri, T value, CancellationToken token = default)
        {
            return await GetProtocol(uri).Write<T>(uri, value, token);
        }

        public async Task Delete(string uri, CancellationToken token = default)
        {
            await GetProtocol(uri).Delete(uri, token);
        }

        private IProtocol GetProtocol(string uri)
        {
            foreach (var protocol in protocols)
            {
                if (uri.StartsWith(protocol.Key))
                {
                    return protocol.Value;
                }
            }

            Debug.LogWarning($"There is not protocol registered for the uri : {uri}. Returning default protocol");

            return defaultProtocol;
        }
    }
}