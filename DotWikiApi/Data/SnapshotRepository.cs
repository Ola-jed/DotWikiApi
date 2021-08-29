using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotWikiApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotWikiApi.Data
{
    public class SnapshotRepository: ISnapshotRepository
    {
        private readonly DotWikiContext _context;

        public SnapshotRepository(DotWikiContext context)
        {
            _context = context;
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Snapshot>> GetSnapshots(int articleId)
        {
            return await _context
                .Snapshots
                .Where(s => s.ArticleId == articleId)
                .ToListAsync();
        }

        public async Task<Snapshot> GetSnapshot(int id)
        {
            return await _context
                .Snapshots
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateSnapshot(Snapshot snapshot)
        {
            await _context.Snapshots.AddAsync(snapshot);
        }

        public void UpdateSnapshot(Snapshot snapshot)
        {
            // Nothing I mean
        }

        public void DeleteSnapshot(Snapshot snapshot)
        {
            _context.Snapshots.Remove(snapshot);
        }
    }
}