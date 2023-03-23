﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Haemimont_Interview.Models
{
    public partial class CourseraContext : DbContext
    {
        public CourseraContext()
        {
        }

        public CourseraContext(DbContextOptions<CourseraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<StudentsCoursesXref> StudentsCoursesXrefs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-2PL5A2F\\SQLEXPRESS;Database=coursera;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.InstructorId).HasColumnName("instructor_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("name");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("time_created")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TotalTime).HasColumnName("total_time");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_courses_instructors");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.ToTable("instructors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("last_name");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("time_created")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Pin);

                entity.ToTable("students");

                entity.Property(e => e.Pin)
                    .HasMaxLength(10)
                    .HasColumnName("pin")
                    .IsFixedLength();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.TimeCreated)
                    .HasColumnType("datetime")
                    .HasColumnName("time_created")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<StudentsCoursesXref>(entity =>
            {
                entity.HasKey(e => new { e.StudentPin, e.CourseId });

                entity.ToTable("students_courses_xref");

                entity.Property(e => e.StudentPin)
                    .HasMaxLength(10)
                    .HasColumnName("student_pin")
                    .IsFixedLength();

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CompletionDate)
                    .HasColumnType("date")
                    .HasColumnName("completion_date");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentsCoursesXrefs)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_students_courses_xref_courses");

                entity.HasOne(d => d.StudentPinNavigation)
                    .WithMany(p => p.StudentsCoursesXrefs)
                    .HasForeignKey(d => d.StudentPin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_students_courses_xref_students");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
