import { Component, OnInit } from '@angular/core';
import { AssuntoService } from '../../../services/assunto.service'; 
import { Assunto } from '../../../models/assunto.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-assunto-lista',
  templateUrl: './assunto-lista.component.html',
  styleUrls: ['./assunto-lista.component.css']
})
export class AssuntoListaComponent implements OnInit {
  assuntos: Assunto[] = [];
  loading = false;
  error: string | null = null;

  constructor(private assuntoService: AssuntoService, private router: Router) { }

  ngOnInit(): void {
    this.loadAssuntos();
  }

  loadAssuntos(): void {
    this.loading = true;
    this.error = null;
    this.assuntoService.getAll().subscribe(
      (assuntos) => {
        this.assuntos = assuntos;
        this.loading = false;
      },
      (error) => {
        this.error = 'Erro ao carregar assuntos.';
        this.loading = false;
        console.error(error);
      }
    );
  }

  editarAssunto(id: number): void {
    this.router.navigate(['/assuntos/editar', id]);
  }

  excluirAssunto(id: number): void {
    if (confirm('Tem certeza que deseja excluir este assunto?')) {
      this.assuntoService.delete(id).subscribe(
        () => {
          this.loadAssuntos();
          alert('Assunto excluído com sucesso!');
        },
        (error) => {
          this.error = 'Erro ao excluir assunto.';
          console.error(error);
        }
      );
    }
  }

  novoAssunto(): void{
    this.router.navigate(['/assuntos/novo']);
  }
}