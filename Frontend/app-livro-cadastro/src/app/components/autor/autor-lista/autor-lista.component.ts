import { Component, OnInit } from '@angular/core';
import { AutorService } from '../../../services/autor.service';
import { Autor } from '../../../models/autor.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-autor-lista',
  templateUrl: './autor-lista.component.html',
  styleUrls: ['./autor-lista.component.css']
})
export class AutorListaComponent implements OnInit {
  autores: Autor[] = [];
  loading = false;
  error: string | null = null;

  constructor(private autorService: AutorService, private router: Router) { }

  ngOnInit(): void {
    this.loadAutores();
  }

  loadAutores(): void {
    this.loading = true;
    this.error = null;
    this.autorService.getAll().subscribe(
      (autores) => {
        this.autores = autores;
        this.loading = false;
      },
      (error) => {
        this.error = 'Erro ao carregar autores.';
        this.loading = false;
        console.error(error);
      }
    );
  }

  editarAutor(id: number): void {
     this.router.navigate(['/autores/editar', id]);
  }

  excluirAutor(id: number): void {
    if (confirm('Tem certeza que deseja excluir este autor?')) {
      this.autorService.delete(id).subscribe(
        () => {
          this.loadAutores();
          alert('Autor excluído com sucesso!');
        },
        (error) => {
          this.error = 'Erro ao excluir autor.';
          console.error(error);
        }
      );
    }
  }
  novoAutor(): void{
    this.router.navigate(['/autores/novo']);
  }
}