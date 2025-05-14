import { Component, OnInit } from '@angular/core';
import { AutorService } from '../../../services/autor.service';
import { Autor } from '../../../models/autor.model';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-autor-form',
  templateUrl: './autor-form.component.html',
  styleUrls: ['./autor-form.component.css']
})
export class AutorFormComponent implements OnInit {
  autorForm: FormGroup;
  autorId: number | null;
  tituloPagina: string;
  loading = false;
  error: string | null = null;

  constructor(
    private autorService: AutorService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.autorForm = this.fb.group({
      nome: ['', Validators.required]
    });
    this.autorId = null;
    this.tituloPagina = 'Novo Autor';
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.autorId = params['id'];
      if (this.autorId) {
        this.tituloPagina = 'Editar Autor';
        this.carregarAutor(this.autorId);
      }
    });
  }

  carregarAutor(id: number): void {
    this.loading = true;
    this.error = null;
    this.autorService.getById(id).subscribe(
    (autor: Autor) => {
        this.autorForm.patchValue(autor);
        this.loading = false;
      },
      (err) => {
        this.error = `Erro ao carregar o autor: ${err.message || 'Erro desconhecido'}`;
        this.loading = false;
        console.error('Erro ao carregar autor:', err);
      }
    );
  }

  salvarAutor(): void {
    if (this.autorForm.invalid) {
      this.autorForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = null;
    const autorData: Autor = this.autorForm.value;

    const operacao = this.autorId
      ? this.autorService.update(this.autorId, autorData)
        : this.autorService.create(autorData);

    operacao.subscribe(
      () => {
        this.loading = false;
        this.router.navigate(['/autores']);
      },
      (err) => {
        this.error = `Erro ao salvar o autor: ${err.message || 'Erro desconhecido'}`;
        this.loading = false;
        console.error('Erro ao salvar autor:', err);
      }
    );
  }

  cancelar(): void {
    this.router.navigate(['/autores']); 
  }
}
