namespace AuctionService.Routes;
public class AuctionRoutes;
public static class AuctionsApi
{
    public const string ApiRoot = "api/auctions";
    public const string IdPlaceholder = "{id}";
    public const string SellerPlaceholder = "{seller}";
    public const string GetAllAuctions = ApiRoot + "/all-auctions";
    public const string GetAuctionById = ApiRoot + "/fetchAuctionById/" + IdPlaceholder;
    public const string GetAuctionBySeller = ApiRoot + "/fetchAuctionBySeller/" + SellerPlaceholder;
    public const string CreateAuction = ApiRoot + "/createAuction";
    public const string UpdateAuction = ApiRoot + "/updateAuction/" + IdPlaceholder;
    public const string DeleteAuction = ApiRoot + "/deleteAuction/" + IdPlaceholder;
}
