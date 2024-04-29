using System;
using wcs.core.model;
using wcs.core.model.Dto;
using wcs.core.service;

namespace wcs.core.common
{
    public class MessageContainer
    {
        const string INFO = "INFO";
        const string ERROR = "ERROR";
        private static MessageContainer _instance;
        private MessageContainer()
        {
            this.messages = new List<MessageItem>();
        }
        public static MessageContainer instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageContainer();
                }

                return _instance;
            }
        }

        private List<MessageItem> messages;
        public List<MessageItem> Messages
        {
            get { return this.messages; }
        }

        private void ClearList()
        {
            if (this.messages.Count > 9)
            {
                this.messages.RemoveRange(9, this.messages.Count - 9);
            }
        }

        public bool Info(string message, string taskId)
        {
            this.ClearList();
            this.messages.Insert(0, new MessageItem()
            {
                type = INFO,
                message = message
            });

            EquipmentRecordsEntity record = new EquipmentRecordsEntity();
            record.message = message;
            record.task_id = taskId;
            record.CreateTime = DateTime.Now;

            Factory<EquipmentRecordsService>.instance.AddRecords(record);

            return true;
        }

        public bool Error(string message, string taskId, string alarmCode)
        {
            this.ClearList();
            this.messages.Insert(0, new MessageItem()
            {
                type = ERROR,
                message = message
            });

            EquipmentRecordsEntity record = new EquipmentRecordsEntity();

            record.CreateTime = DateTime.Now;
            record.task_id = taskId;
            record.message = message;
            record.alarm_code = "SYSERROR";
            record.alarm_type = "系统类异常";

            if (!string.IsNullOrEmpty(alarmCode))
            {
                EquipmentAlarmTypeDto type = Factory<EquipmentAlarmService>.instance.getAlarmByCode(alarmCode);

                if (type != null)
                {
                    record.message = $"{type.alarm_remark}-{message}";
                    record.alarm_code = type.alarm_code;
                    record.alarm_level = type.alarm_level;
                    record.alarm_type = type.alarm_type;
                }
            }

            Factory<EquipmentRecordsService>.instance.AddRecords(record);

            return true;
        }
    }
}

