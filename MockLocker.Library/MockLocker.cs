namespace MockLocker.Library
{
    using global::MockLocker.Library.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO.Ports;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class MockLocker
    {
        private readonly LockType _lockType;
        private readonly AppSettingsManager _appSettings;
        public MockLocker(LockType lockType)
        {
            _lockType = lockType;
            _appSettings = new AppSettingsManager();
        }

        // This internal constructor is only for testing purposes
        internal MockLocker(LockType lockType, AppSettingsManager appSettings)
        {
            _lockType = lockType;
            _appSettings = appSettings;
        }

        public Response Unlock(int lockNo)
        {
            Response response = new() { Error = "Invalid Lock No !" };
            try
            {
                switch (_lockType, lockNo)
                {
                    case (_, _) when lockNo < 1:
                        response.IsSuccess = false;
                        break;
                    case (_, _) when _lockType == LockType.Lock16 && lockNo > 16:
                        response.IsSuccess = false;
                        break;
                    case (_, _) when _lockType == LockType.Lock32 && lockNo > 32:
                        response.IsSuccess = false;
                        break;
                    case (_, _) when _lockType == LockType.Lock48 && lockNo > 48:
                        response.IsSuccess = false;
                        break;
                    case (_, _) when _lockType == LockType.Lock64 && lockNo > 64:
                        response.IsSuccess = false;
                        break;
                    default:
                        response.IsSuccess = true;
                        response.Error = string.Empty;
                        break;
                }
                if (response.IsSuccess == false)
                    return response;
                _appSettings.UpdateAppSettings($"LockNoStatus.{lockNo}", 0);
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = e.Message;
            }
            return response;
        }


        public CheckStatusResponse CheckStatus()
        {
            CheckStatusResponse response = new();
            List<Status> statuses = new();
            int locNo = 0;

            try
            {
                List<string> locStatusList = _appSettings.GetJsonListFromSettings("LockNoStatus");

                int numOfLocks = _lockType switch
                {
                    LockType.Lock16 => 16,
                    LockType.Lock32 => 32,
                    LockType.Lock48 => 48,
                    LockType.Lock64 => 64,
                };

                locStatusList.Take(numOfLocks).ToList().ForEach(x =>
                {
                    statuses.Add(new Status() { LockNo = ++locNo, IsLocked = (x == "1") });
                });

                response.Statuses = statuses;
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = e.Message;
            }
            return response;
        }
        public Response CloseAllLocks()
        {
            Response response = new();

            try
            {
                int numOfLocks = _lockType switch
                {
                    LockType.Lock16 => 16,
                    LockType.Lock32 => 32,
                    LockType.Lock48 => 48,
                    LockType.Lock64 => 64,
                };

                for (int i = 1; i <= numOfLocks; i++)
                {
                    _appSettings.UpdateAppSettings($"LockNoStatus.{i}", 1);
                }
                response.IsSuccess = true;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = e.Message;
            }

            return response;
        }
    }
}
