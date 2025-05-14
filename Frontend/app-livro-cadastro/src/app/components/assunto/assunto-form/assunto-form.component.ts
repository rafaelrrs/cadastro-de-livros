import { Component, OnInit } from '@angular/core';
import { AssuntoService } from '../../../services/assunto.service';
import { Assunto } from '../../../models/assunto.model';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-assunto-form',
  imports: [CommonModule, ReactiveFormsModule],
  standalone: true,
  templateUrl: './assunto-form.component.html',
  styleUrls: ['./assunto-form.component.css']
})
export class AssuntoFormComponent implements OnInit {
  assuntoForm: FormGroup;
  assuntoId: number | null;
  tituloPagina: string;
  loading = false;
  error: string | null = null;

  constructor(
    private assuntoService: AssuntoService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.assuntoForm = this.fb.group({
      descricao: ['', Validators.required]
    });
    this.assuntoId = null;
    this.tituloPagina = 'Novo Assunto';
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.assuntoId = params['id'];
      if (this.assuntoId) {
        this.tituloPagina = 'Editar Assunto';
        this.carregarAssunto(this.assuntoId);
      }
    });
  }

  carregarAssunto(id: number): void {
    this.loading = true;
    this.error = null;
    this.assuntoService.getById(id).subscribe(
      (assunto) => {
        this.assuntoForm.patchValue({
          descricao: assunto.descricao
        });
        this.loading = false;
      },
      (error) => {
        this.error = 'Erro ao carregar assunto.';
        this.loading = false;
        console.error(error);
      }
    );
  }

  salvarAssunto(): void {
    if (this.assuntoForm.valid) {
      this.loading = true;
      this.error = null;
      const assunto = this.assuntoForm.value as Assunto;
      if (this.assuntoId) {
        this.assuntoService.update(this.assuntoId, assunto).subscribe(
          () => {
            alert('Assunto atualizado com sucesso!');
            this.router.navigate(['/assuntos']);
            this.loading = false;
          },
          (error) => {
            this.error = 'Erro ao atualizar assunto.';
            this.loading = false;
            console.error(error);
          }
        );
      } else {
        this.assuntoService.create(assunto).subscribe(
          () => {
            alert('Assunto criado com sucesso!');
            this.router.navigate(['/assuntos']);
            this.loading = false;
          },
          (error) => {
            this.error = 'Erro ao criar assunto.';
            this.loading = false;
            console.error(error);
          }
        );
      }
    }
  }

  cancelar(): void {
    this.router.navigate(['/assuntos']);
  }
}
