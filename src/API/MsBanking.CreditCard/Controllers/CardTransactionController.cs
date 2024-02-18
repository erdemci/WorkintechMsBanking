using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MsBanking.Card.Domain.Dto;
using MsBanking.Card.Services;

namespace MsBanking.Card.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardTransactionController : ControllerBase
    {
        private readonly ICardTransactionService _cardTransactionService;
        private readonly IMapper _mapper;

        public CardTransactionController(ICardTransactionService cardTransactionService, IMapper mapper)
        {
            _cardTransactionService = cardTransactionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<CardTransactionResponseDto> Post(CardTransactionRequestDto cardTransactionRequestDto)
        {
            var result = await _cardTransactionService.CreateCardTransaction(cardTransactionRequestDto);
            return result;
        }
    }
}
