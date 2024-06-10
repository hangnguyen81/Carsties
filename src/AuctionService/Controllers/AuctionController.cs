using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuctionService.Routes;

namespace AuctionService.Controller;
[ApiController]
public class AuctionController : ControllerBase
{
    private readonly AuctionDbContext _dbContext;
    private readonly IMapper _mapper;
    public AuctionController(AuctionDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet]
    [Route(AuctionsApi.GetAllAuctions)]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _dbContext.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
    }
    
    [HttpGet]
    [Route(AuctionsApi.GetAuctionById)]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _dbContext.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();
        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpGet]
    [Route(AuctionsApi.GetAuctionBySeller)]
    public async Task<ActionResult<List<AuctionDto>>> GetAuctionsBySeller(string seller)
    {
        var auctions = await _dbContext.Auctions
            .Include(x => x.Item)
            .Where(x => x.Seller == seller)
            .ToListAsync();

        if (auctions == null) return NotFound();
        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpPost]
    [Route(AuctionsApi.CreateAuction)]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);
        //TODO: add current user as seller
        auction.Seller = "Test";

        _dbContext.Auctions.Add(auction);
        var result = await _dbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not saved changes to DB");
        return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id }, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut]
    [Route(AuctionsApi.UpdateAuction)]
    public async Task<ActionResult> UpdateAution(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _dbContext.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (auction == null) return NotFound();

        //TODO: check seller == username
        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;

        var result = await _dbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not saved changes to DB");
        return NoContent();
    }

    [HttpDelete]
    [Route(AuctionsApi.DeleteAuction)]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _dbContext.Auctions.FindAsync(id);
        if (auction == null) return NotFound();
        //TODO: check seller == username
        _dbContext.Auctions.Remove(auction);
        var result = await _dbContext.SaveChangesAsync() > 0;
        if (!result) return BadRequest("Could not update DB");
        return NoContent();
    }
}