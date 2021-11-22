using System.Collections.Generic;

namespace SteinDataAccess
{
    public interface IDataContext
    {
        #region Inventory

        List<Inventory> GetInventories();
        Inventory GetInventory(int id);
        void AddInventory(Inventory inventory);
        void UpdateInventory(Inventory inventory);
        void DeleteInventory(int id);

        #endregion

        #region Request

        List<Request> GetRequests();
        Request GetRequest(int id);
        void AddRequest(Request request);
        void UpdateRequest(Request request);
        void DeleteRequest(int id);

        #endregion

        void SaveChanges();
    }
}