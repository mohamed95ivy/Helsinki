import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConversionRequestDto, ConversionResponseDto, PaginationResult } from '../models/conversion.models';


@Injectable({ providedIn: 'root' })
export class ApiService {
    private http = inject(HttpClient);

    // state: history
    history = signal<ConversionResponseDto[]>([]);


    convert(req: ConversionRequestDto) {
        return this.http.post<ConversionResponseDto>(`/api/conversion`, req);
    }


    getCurrencies() {
        return this.http.get<string[]>(`/api/currencies`);
    }


    getRates(base: string) {
        return this.http.get<Record<string, number>>(`/api/rates/${base}`);
    }


    loadHistory(params: { skip?: number; take?: number; sort?: 'asc' | 'desc'; userId?: string; }) {
        const q = new URLSearchParams({
            skip: String(params.skip ?? 0),
            take: String(params.take ?? 50),
            sort: params.sort ?? 'desc',
            userId: params.userId ?? 'candidate'
        });
        return this.http.get<PaginationResult>(`/api/conversion/history?${q.toString()}`);
    }
}