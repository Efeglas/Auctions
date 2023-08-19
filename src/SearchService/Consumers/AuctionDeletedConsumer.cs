using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
    {
        private readonly IMapper _mapper;

        public AuctionDeletedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine($"--> Consuming auction deleted: {context.Message.Id}");
            Item item = _mapper.Map<Item>(context.Message);
            DeleteResult result = await item.DeleteAsync();

            if (!result.IsAcknowledged)
            {
                throw new MessageException(typeof(AuctionDeleted), "Problem deleting from MongoDB");
            }
        }
    }
}
