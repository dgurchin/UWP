#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System.Threading.Tasks;

using Inventory.Common;

namespace Inventory.Services
{
    public enum DataProviderType
    {
        SQLite,
        SQLServer,
        WebAPI
    }

    public interface ISettingsService
    {
        string Version { get; }
        string DbVersion { get; }

        string UserName { get; set; }

        DataProviderType DataProvider { get; set; }
        string PatternConnectionString { get; }
        string SQLiteConnectionString { get; }
        string SQLServerConnectionString { get; set; }
        bool IsRandomErrorsEnabled { get; set; }
        string SipPhoneName { get; set; }
        string SipPhonePassword { get; set; }
        string SipPhoneDomain { get; set; }
        string SipPhoneTransportProtocol { get; set; }
        string SipPhonePort { get; set; }

        string MonitorHost { get; set; }
        string MonitorUserName { get; set; }
        string MonitorPassword { get; set; }

        Task<Result> ResetLocalDataProviderAsync();

        Task<Result> ValidateConnectionAsync(string connectionString);
        Task<Result> CreateDabaseAsync(string connectionString);
        Task<Result> ApplyMigrationAsync(string connectionString);
        Task<Result> CopyProductImagesAsync(string connectionString);
    }
}
