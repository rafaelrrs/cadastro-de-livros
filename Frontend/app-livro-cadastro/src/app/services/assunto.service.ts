import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Assunto } from '../models/assunto.model';

@Injectable({
  providedIn: 'root'
})
export class AssuntoService {
  private apiUrl = 'http://localhost:5000/api/assuntos';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Assunto[]> {
    return this.http.get<Assunto[]>(this.apiUrl);
  }

  getById(id: number): Observable<Assunto> {
    return this.http.get<Assunto>(`${this.apiUrl}/${id}`);
  }

  create(assunto: Assunto): Observable<Assunto> {
    return this.http.post<Assunto>(this.apiUrl, assunto);
  }

  update(id: number, assunto: Assunto): Observable<Assunto> {
    return this.http.put<Assunto>(`${this.apiUrl}/${id}`, assunto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}