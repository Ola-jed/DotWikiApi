using System.Collections.Generic;
using System.Threading.Tasks;
using DotWikiApi.Models;

namespace DotWikiApi.Data.Contracts;

public interface ISnapshotRepository
{
    Task SaveChanges();
    Task<IEnumerable<Snapshot>> GetSnapshots(int articleId);
    Task<Snapshot?> GetSnapshot(int id);
    Task CreateSnapshot(Snapshot snapshot);
    void UpdateSnapshot(Snapshot _);
    void DeleteSnapshot(Snapshot snapshot);
}