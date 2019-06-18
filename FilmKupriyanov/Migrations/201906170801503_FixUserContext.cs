namespace FilmKupriyanov.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Year = c.Int(nullable: false),
                        Director = c.String(),
                        CreatorUserId = c.String(nullable: false, maxLength: 128),
                        Poster = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorUserId, cascadeDelete: true)
                .Index(t => t.CreatorUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Films", "CreatorUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Films", new[] { "CreatorUserId" });
            DropTable("dbo.Films");
        }
    }
}
