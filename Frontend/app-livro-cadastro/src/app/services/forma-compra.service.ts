import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FormaCompra } from '../models/forma-compra.model';

@Injectable({
  providedIn: 'root'
})
export class FormaCompraService {
  private apiUrl = 'http://localhost:5000/api/formacompra';

  constructor(private http: HttpClient) { }

  getAll(): Observable<FormaCompra[]> {
    return this.http.get<FormaCompra[]>(this.apiUrl);
  }

  getById(id: number): Observable<FormaCompra> {
    return this.http.get<FormaCompra>(`${this.apiUrl}/${id}`);
  }
}