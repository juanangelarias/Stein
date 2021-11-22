using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SteinCommon;

namespace SteinDataAccess
{
    public class DataContext: IDataContext
    {
        private bool _loaded;
        private string _dataJson;
        private List<Inventory> _inventories;
        private List<Request> _requests;

        #region Inventory

        public List<Inventory> GetInventories()
        {
            if(!_loaded)
                ReadDb();

            return _inventories;
        }

        public Inventory GetInventory(int id)
        {
            if(!_loaded)
                ReadDb();

            return _inventories.FirstOrDefault(r => r.Id == id);
        }

        public void AddInventory(Inventory inventory)
        {
            if(!_loaded)
                ReadDb();

            _inventories.Add(inventory);
        }

        public void UpdateInventory(Inventory inventory)
        {
            if(!_loaded)
                ReadDb();

            var record = _inventories.FirstOrDefault(r => r.Id == inventory.Id);

            if (record == null)
                throw new NotFoundException("Inventory Not Found");

            record.Name = inventory.Name;
            record.Kernels = inventory.Kernels;
        }

        public void DeleteInventory(int id)
        {
            if(!_loaded)
                ReadDb();

            var record = _inventories.FirstOrDefault(r => r.Id == id);

            if (record == null)
                throw new NotFoundException("Inventory Not Found");

            _inventories.Remove(record);
        }

        #endregion

        #region Request

        public List<Request> GetRequests()
        {
            if (!_loaded)
                ReadDb();

            return _requests;
        }

        public Request GetRequest(int id)
        {
            if (!_loaded)
                ReadDb();

            return _requests.FirstOrDefault(r => r.Id == id);
        }

        public void AddRequest(Request request)
        {
            if (!_loaded)
                ReadDb();

            _requests.Add(request);
        }

        public void UpdateRequest(Request request)
        {
            if (!_loaded)
                ReadDb();

            var record = _requests.FirstOrDefault(r => r.Id == request.Id);

            if (record == null)
                throw new NotFoundException("Request Not Found");

            record.InventoryId = record.InventoryId;
            record.RequestedKernels = record.RequestedKernels;
        }

        public void DeleteRequest(int id)
        {
            if (!_loaded)
                ReadDb();

            var record = _requests.FirstOrDefault(r => r.Id == id);

            if (record == null)
                throw new NotFoundException("Request Not Found");

            _requests.Remove(record);
        }

        #endregion

        public void SaveChanges()
        {
            SerializeJson();
            
            File.Delete("db.json");
            File.WriteAllText(_dataJson, "db.json");
        }

        private void ReadDb()
        {
            ReadJsonFile();
            DeserializeJson();
            _loaded = true;
        }

        private void ReadJsonFile()
        {
            var json = File.ReadAllText("db.json");
            _dataJson = json;
        }

        private void DeserializeJson()
        {
            var data = JsonConvert.DeserializeObject<XData>(_dataJson);

            if (data == null) 
                return;

            _inventories = data.Inventory.Select(item => new Inventory
            {
                Id = item.Id,
                Name = item.Name,
                Kernels = item.Kernels
            }).ToList();

            _requests = data.Requests.Select(item => new Request
            {
                Id = item.Id,
                InventoryId = Convert.ToInt32(item.InventoryId),
                RequestedKernels = item.RequestedKernels
            }).ToList();
        }

        private void SerializeJson()
        {
            var xData = new XData
            {
                Inventory = _inventories.Select(item => new XInventory
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Kernels = item.Kernels
                    })
                    .ToList(),
                Requests = _requests.Select(item => new XRequest
                    {
                        Id = item.Id,
                        InventoryId = item.InventoryId.ToString(),
                        RequestedKernels = item.RequestedKernels
                    })
                    .ToList()
            };

            _dataJson = JsonConvert.SerializeObject(xData, Formatting.Indented);
        }
    }

    public class XInventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Kernels { get; set; }
    }

    public class XRequest
    {
        public int Id { get; set; }
        public string InventoryId { get; set; }
        public int RequestedKernels { get; set; }
    }

    public class XData
    {
        public List<XInventory> Inventory { get; set; }
        public List<XRequest> Requests { get; set; }
    }
}