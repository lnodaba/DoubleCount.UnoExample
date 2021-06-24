using DCx.lib.Webshop.Storage.Services;
using DCx.Webshop.Models;
using System;
using System.Collections.Generic;

namespace DCx.Webshop.Services.Tickets
{
    public class PredefinedMessagesHandler
    {
        private readonly IDatabaseSettings _settings;
        private readonly BaseMongoDBService<PredefinedMessage> _service;

        public PredefinedMessagesHandler(IDatabaseSettings settings)
        {
            _settings = settings;
            _service = new BaseMongoDBService<PredefinedMessage>(settings);
            AddDefaults();
        }

        public List<PredefinedMessage> GetAll()
        {
            return _service.Get();
        }

        public void AddDefaults()
        {
            if (_service.Get().Count == 0)
            {
                _service.Create(new PredefinedMessage()
                {
                    Label = "Not enough information",
                    Message = "Please provide more information"
                });
                _service.Create(new PredefinedMessage()
                {
                    Label = "Missing information",
                    Message = "Please add more information"
                });
            }
        }

        public void Add(PredefinedMessage message)
        {
            _service.Create(message);
        }

        public void Update(PredefinedMessage message)
        {
            _service.Update(message.Id, message);
        }

        public void Remove(PredefinedMessage message)
        {
            _service.Remove(message);
        }
    }
}
