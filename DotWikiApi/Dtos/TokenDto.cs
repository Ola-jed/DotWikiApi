using System;

namespace DotWikiApi.Dtos;

public record TokenDto(string Token, DateTime Expiration);