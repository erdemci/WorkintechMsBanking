using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MsBanking.Common.Dto;
using MsBanking.Core.Branch.Domain;

namespace MsBanking.Core.Branch.Services
{
    public class BranchService : IBranchService
    {
        private readonly BranchDbContext db;
        private readonly IMapper mapper;

        public BranchService(BranchDbContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }

        public async Task<List<BranchResponseDto>> GetBranchesAsync()
        {
            var branches = await db.Branches.ToListAsync();

            var mapped = mapper.Map<List<BranchResponseDto>>(branches);

            return mapped;
        }

        public async Task<BranchResponseDto> GetBranchByIdAsync(int id)
        {
            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            var mapped = mapper.Map<BranchResponseDto>(branch);

            return mapped;
        }

        public async Task<BranchResponseDto> CreateBranchAsync(BranchDto branchDto)
        {
            var branch = mapper.Map<MsBanking.Common.Entity.Branch>(branchDto);

            db.Branches.Add(branch);
            await db.SaveChangesAsync();

            var mapped = mapper.Map<BranchResponseDto>(branch);

            return mapped;
        }

        public async Task<BranchResponseDto> UpdateBranchAsync(int id, BranchDto branchDto)
        {
            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            if (branch == null)
            {
                return null;
            }

            mapper.Map(branchDto, branch);

            db.Branches.Update(branch);
            await db.SaveChangesAsync();

            var mapped = mapper.Map<BranchResponseDto>(branch);

            return mapped;
        }

        public async Task<bool> DeleteBranchAsync(int id)
        {
            var branch = await db.Branches.FirstOrDefaultAsync(x => x.Id == id);

            if (branch == null)
            {
                return false;
            }

            db.Branches.Remove(branch);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
