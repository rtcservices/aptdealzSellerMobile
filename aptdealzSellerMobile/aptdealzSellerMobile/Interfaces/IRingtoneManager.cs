﻿using System.Collections.Generic;

namespace aptdealzSellerMobile.Interfaces
{
    public interface IRingtoneManager
    {
        Dictionary<int, string> GetRingtones();

        void PlayRingTone(int Id);

        //void StopRingTone(int Id);

        void SaveRingTone(int Id);

        //string OpenNotificationSettings();
    }
}
