-- MySQL Script generated by MySQL Workbench
-- 07/20/15 19:44:09
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema supermarkets_chain
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema supermarkets_chain
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `supermarkets_chain` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `supermarkets_chain` ;

-- -----------------------------------------------------
-- Table `supermarkets_chain`.`measures`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`measures` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `name` NVARCHAR(50) NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '')
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`vendors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`vendors` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `name` NVARCHAR(50) NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '')
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`products`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`products` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `name` NVARCHAR(50) NOT NULL COMMENT '',
  `price` DECIMAL(8,2) NOT NULL COMMENT '',
  `measure_id` INT NOT NULL COMMENT '',
  `vendor_id` INT NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '',
  INDEX `fk_products_measures_idx` (`measure_id` ASC)  COMMENT '',
  INDEX `fk_products_vendors1_idx` (`vendor_id` ASC)  COMMENT '',
  CONSTRAINT `fk_products_measures`
    FOREIGN KEY (`measure_id`)
    REFERENCES `supermarkets_chain`.`measures` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_products_vendors1`
    FOREIGN KEY (`vendor_id`)
    REFERENCES `supermarkets_chain`.`vendors` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`expenses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`expenses` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `month` DATE NOT NULL COMMENT '',
  `expense` DECIMAL(8,2) NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '')
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`vendors_has_expenses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`vendors_has_expenses` (
  `vendor_id` INT NOT NULL COMMENT '',
  `expense_id` INT NOT NULL COMMENT '',
  INDEX `fk_vendors_has_expenses_expenses1_idx` (`expense_id` ASC)  COMMENT '',
  INDEX `fk_vendors_has_expenses_vendors1_idx` (`vendor_id` ASC)  COMMENT '',
  CONSTRAINT `fk_vendors_has_expenses_vendors1`
    FOREIGN KEY (`vendor_id`)
    REFERENCES `supermarkets_chain`.`vendors` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_vendors_has_expenses_expenses1`
    FOREIGN KEY (`expense_id`)
    REFERENCES `supermarkets_chain`.`expenses` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`supermarkets`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`supermarkets` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `name` NVARCHAR(50) NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '')
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`supermarkets_has_products`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`supermarkets_has_products` (
  `supermarket_id` INT NOT NULL COMMENT '',
  `product_id` INT NOT NULL COMMENT '',
  PRIMARY KEY (`supermarket_id`, `product_id`)  COMMENT '',
  INDEX `fk_supermarkets_has_products_products1_idx` (`product_id` ASC)  COMMENT '',
  INDEX `fk_supermarkets_has_products_supermarkets1_idx` (`supermarket_id` ASC)  COMMENT '',
  CONSTRAINT `fk_supermarkets_has_products_supermarkets1`
    FOREIGN KEY (`supermarket_id`)
    REFERENCES `supermarkets_chain`.`supermarkets` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_supermarkets_has_products_products1`
    FOREIGN KEY (`product_id`)
    REFERENCES `supermarkets_chain`.`products` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `supermarkets_chain`.`sales`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `supermarkets_chain`.`sales` (
  `id` INT NOT NULL AUTO_INCREMENT COMMENT '',
  `ordered_on` DATE NOT NULL COMMENT '',
  `supermarket_id` INT NOT NULL COMMENT '',
  `product_id` INT NOT NULL COMMENT '',
  PRIMARY KEY (`id`)  COMMENT '',
  INDEX `fk_sales_supermarkets1_idx` (`supermarket_id` ASC)  COMMENT '',
  INDEX `fk_sales_products1_idx` (`product_id` ASC)  COMMENT '',
  CONSTRAINT `fk_sales_supermarkets1`
    FOREIGN KEY (`supermarket_id`)
    REFERENCES `supermarkets_chain`.`supermarkets` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_sales_products1`
    FOREIGN KEY (`product_id`)
    REFERENCES `supermarkets_chain`.`products` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
