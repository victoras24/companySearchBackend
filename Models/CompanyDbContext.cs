using System;
using System.Collections.Generic;
using CompanySearchBackend.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CompanySearchBackend.Models;

public partial class CompanyDbContext : DbContext
{
    public CompanyDbContext()
    {
    }

    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CompanyNameDto> Organisations { get; set; }

    public virtual DbSet<OrganisationOfficial> OrganisationOfficials { get; set; }

    public virtual DbSet<RegisteredOffice> RegisteredOffices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organisations>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("organisations");

            entity.Property(e => e.AddressSeqNumber).HasColumnName("Address_Seq_Number");
            entity.Property(e => e.ExtraColumn)
                .HasMaxLength(100)
                .HasColumnName("Extra_Column");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NameStatus)
                .HasMaxLength(100)
                .HasColumnName("Name_Status");
            entity.Property(e => e.NameStatusCode)
                .HasMaxLength(100)
                .HasColumnName("Name_Status_Code");
            entity.Property(e => e.OrganisationStatus)
                .HasMaxLength(100)
                .HasColumnName("Organisation_Status");
            entity.Property(e => e.OrganisationStatusDate).HasColumnName("Organisation_Status_Date");
            entity.Property(e => e.OrganisationSubType)
                .HasMaxLength(100)
                .HasColumnName("Organisation_Sub_Type");
            entity.Property(e => e.OrganisationType)
                .HasMaxLength(100)
                .HasColumnName("Organisation_Type");
            entity.Property(e => e.OrganisationTypeCode)
                .HasMaxLength(100)
                .HasColumnName("Organisation_Type_Code");
            entity.Property(e => e.RegistrationDate).HasColumnName("Registration_Date");
            entity.Property(e => e.RegistrationNo).HasColumnName("Registration_No");
        });

        modelBuilder.Entity<OrganisationOfficial>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("organisation_officials");

            entity.Property(e => e.OfficialPosition)
                .HasMaxLength(100)
                .HasColumnName("OFFICIAL_POSITION");
            entity.Property(e => e.OrganisationName)
                .HasMaxLength(100)
                .HasColumnName("ORGANISATION_NAME");
            entity.Property(e => e.OrganisationType)
                .HasMaxLength(100)
                .HasColumnName("ORGANISATION_TYPE");
            entity.Property(e => e.OrganisationTypeCode)
                .HasMaxLength(100)
                .HasColumnName("ORGANISATION_TYPE_CODE");
            entity.Property(e => e.PersonOrOrganisationName)
                .HasMaxLength(100)
                .HasColumnName("PERSON_OR_ORGANISATION_NAME");
            entity.Property(e => e.RegistrationNo)
                .HasMaxLength(100)
                .HasColumnName("REGISTRATION_NO");
        });

        modelBuilder.Entity<RegisteredOffice>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("registered_office");

            entity.Property(e => e.AddressSeqNo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ADDRESS_SEQ_NO");
            entity.Property(e => e.Building)
                .HasMaxLength(100)
                .HasColumnName("BUILDING");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("STREET");
            entity.Property(e => e.Territory)
                .HasMaxLength(100)
                .HasColumnName("TERRITORY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
