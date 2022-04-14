using System;

namespace ProbesApi.HealthCheck
{
    public class ApiHealthState
    {
        bool _isHealthy = true;
        public bool IsHealthy 
        {
            get
            {
                if (!_isHealthy)
                {
                    if (_dateAutoRecoveryFromNow < DateTime.Now)
                    {
                        _isHealthy = true;
                        return _isHealthy;
                    }
                }

                return _isHealthy;
            }
            set
            {
                _isHealthy = value;                
            }
        }


        DateTime _dateAutoRecoveryFromNow;
        int _autoRecoveryAfterSeconds = 0;
        public int AutoRecoveryAfterSeconds 
        {
            get => _autoRecoveryAfterSeconds;
            
            set
            {
                _autoRecoveryAfterSeconds = value;
                _dateAutoRecoveryFromNow = DateTime.Now.AddSeconds(value);
            }
        }        
    }
}
