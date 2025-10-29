export interface ConversionRequestDto {
    fromCurrency: string;
    toCurrency: string;
    amount: number;
    userId?: string | null;
}


export interface ConversionResponseDto {
    conversionId: string;
    fromCurrency: string;
    toCurrency: string;
    fromAmount: number;
    toAmount: number;
    exchangeRate: number;
    conversionDate: string; // ISO
}

export interface PaginationResult{
    items: ConversionResponseDto[];
    count: number;
}